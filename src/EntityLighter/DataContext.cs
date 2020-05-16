/*
** 
**  electrifier - EntityLighter
** 
**  Copyright 2017-19 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/



using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Diagnostics;


// TODO: 10/05/20: Use events for Error-Handling to enable easy overwriting
// TODO: 10/05/20: https://www.codeproject.com/Articles/43025/A-LINQ-Tutorial-Mapping-Tables-to-Objects
// TODO: Currently we don't support multi-column primary keys! https://www.sqlitetutorial.net/sqlite-primary-key/
// TODO: We may use INotifyPropertyChanged for updating in real-time
// TODO: See https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/attribute-based-mapping,
//           https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.mapping?view=netframework-4.8 for precise object names


namespace EntityLighter
{
    /// <summary>
    /// Enumeration of supported <see cref="DataType"/> each entity property can have.<br/>
    /// <br/>
    /// See <seealso href="https://www.sqlite.org/datatype3.html"/> for further information.
    /// </summary>
    public enum DataType
    {
        Integer,
        Real,
        Text,
        Blob,
    }

    /// <summary>
    /// Enumeration of supported <see cref="Constraint"/> flags each entity property can have.<br/>
    /// <br/>
    /// See <seealso href="https://www.tutorialspoint.com/sqlite/sqlite_constraints.htm"/> for further information.
    /// </summary>
    [Flags]
    public enum Constraint
    {
        None = 0x0,
        NotNull = 0x10,
        Unique = 0x20,
        PrimaryKey = 0x40,
    }

    #region Extensions ========================================================================================================

    internal static class EntityStoreExtensions
    {
        public static string ToSQLStatement(this Constraint constraint)
        {
            return (constraint.HasFlag(Constraint.NotNull) ? "NOT NULL " : "")
                + (constraint.HasFlag(Constraint.Unique) ? "UNIQUE " : "")
                + (constraint.HasFlag(Constraint.PrimaryKey) ? "PRIMARY KEY" : "");
        }
    }

    #endregion ================================================================================================================



    #region Attributes ========================================================================================================

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnAttribute : Attribute
    {
        public ColumnAttribute(DataType dataType)
        {
            this.DataType = dataType;
        }

        // Positional, Mandatory Argument
        public DataType DataType { get; }

        // Optional, Named Arguments
        public Constraint Constraints { get; set; }
        public string DefaultValue { get; set; }

        /// <summary>
        /// Generate SQL-Snippet describing this property for use in CREATE TABLE-Statement
        /// </summary>
        /// <returns></returns>
        public string ToSQLSnippet(string columnName)
        {
            StringBuilder stmt = new StringBuilder($"{ columnName } { this.DataType } { this.Constraints.ToSQLStatement() } ");

            if (!string.IsNullOrWhiteSpace(this.DefaultValue))
                stmt.Append($"DEFAULT { this.DefaultValue }");

            return stmt.ToString();
        }
    }

    #endregion ================================================================================================================

    #region Interfaces ========================================================================================================

    public interface ILightedEntity
    {
        DataContext DataContext { get; }
        string DatabaseTableName { get; }           // TODO: 01/05/20 Replace this by an attribute
        long Id { get; }        // TODO: Replace this by it's rowid. rowid=null, entity is in creation
    }

    #endregion ================================================================================================================

    public class DataContext
      : IDisposable
    {
        #region Fields ========================================================================================================

        private readonly object databaseLock = new object();

        private const uint sqlUserVersionID = 0xE1EC0101;
        // TODO: pragma application_id seems to be unavailable with Microsoft.Data.Sqlite => SQLitePCL.sqlite3_stmt ?!?
        //        private const uint sqlApplicationID = 0xE1EC781F;

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public SqliteConnection SqliteConnection { get; private set; }

        public string Storage { get; }

        #endregion ============================================================================================================


        //public string SQLiteVersionId
        //{
        //    get
        //    {
        //        SqliteCommand sqliteCommand = this.SqliteConnection.CreateCommand();

        //        sqliteCommand.CommandText = "SELECT 'SQLite v' || SQLite_Version()";

        //        return sqliteCommand.ExecuteScalar().ToString();
        //    }
        //}

        private class EntityStatementBuilder
        {
            public string EntityName { get; }

            private readonly List<Tuple<string, ColumnAttribute>> TableColumns = new List<Tuple<string, ColumnAttribute>>();

            public EntityStatementBuilder(string entityName)
            {
                this.EntityName = entityName;
            }

            public void AddAttribute(string columnName, ColumnAttribute attribute)
            {
                this.TableColumns.Add(new Tuple<string, ColumnAttribute>(columnName, attribute));
            }

            public string ToSQLStatement()
            {
                int attributeCount = this.TableColumns.Count;

                if (attributeCount < 1)
                    return string.Empty;

                StringBuilder statement = new StringBuilder($"CREATE TABLE IF NOT EXISTS '{ this.EntityName }' ( ");

                for (int i = 0; i < attributeCount;)
                {
                    this.TableColumns[i].Deconstruct(out string columnName, out ColumnAttribute attribute);

                    statement.Append(attribute.ToSQLSnippet(columnName));
                    statement.Append(++i < attributeCount ? ", " : ")");
                }

                return statement.ToString();
            }
        }



        // TODO: 10/05/20 Perhaps use an event for this?
        public delegate void SetEntityCreationParamsCallback(SqliteCommand sqliteCommand);


        public DataContext(string storage, Type[] types) // CreateIfNotExists-param // TODO: Move types to AddEntityTable-Method!
        {
            this.Storage = storage ?? throw new ArgumentNullException(nameof(storage));

            if (types is null)
                throw new ArgumentNullException(nameof(types));

            try
            {
                this.SqliteConnection = new SqliteConnection(
                    new SqliteConnectionStringBuilder()
                    {
                        DataSource = this.Storage,
                        //Mode = SqliteOpenMode.Memory,
                    }.ToString()
                );

                // This is how you could 'embed' C#-functions into SQLite!
                // https://stackoverflow.com/questions/24229785/sqlite-net-sqlitefunction-not-working-in-linq-to-sql/26155359#26155359
                // https://docs.microsoft.com/de-de/dotnet/standard/data/sqlite/user-defined-functions
                //this.SqliteConnection.CreateFunction<>

                this.SqliteConnection.Open();

                // Check if Database model already exists
                using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
                {
                    // TODO: TEST-Code!

                    // TODO: May use  'sqlite_master' table for stuff like this!

                    sqlCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Session';"; // TODO: name in ('Session', 'Workbench', ... put all tables into it); AND verify UserVersionID!
                    var res = sqlCmd.ExecuteReader();

                    //if (res.HasRows)
                    //    System.Windows.Forms.MessageBox.Show("Database already defined!");
                }

                /*
                 * Create new database model
                 */

                // TODO: pragma application_id seems to be unavailable with Microsoft.Data.Sqlite
                //       Perhaps give another try with native SQLite
                //this.SetPragmaValue("application_id", sqlApplicationID.ToString());

                this.SetPragmaValue("user_version", sqlUserVersionID.ToString(CultureInfo.InvariantCulture));
                foreach (var type in types)
                    this.CreateEntityModel(type);

                //this.ExecuteNonQuery(EntityLighter.SqlStmtCreateDatabase);



                //          // TODO: 17.04.2020!!! Fehlerhandling!
                //
                //
                //
                //                ElSessionEntity session = new ElSessionEntity(this);        // TODO: Exception: "Obsolete!" => Keine vernünftige Fehlermeldung!
                //
                //
                //              // throw new Exception("Ätsch!"); => Der Startcode fällt beim LADEN schon auf die Füsse, nicht beim Ausführen...
                //



                //                SessionEntity sessionEntity = new SessionEntity(this);

            }
            catch (SqliteException)
            {
                //var ext = "SQL - Exception: " + ex.Message;

                //if (null != ex.InnerException)
                //    ext += "\n\nInner: " + ex.InnerException.Message;

                //System.Windows.Forms.MessageBox.Show(ext);
                throw;
            }
        }

        public int ExecuteNonQuery(string statement)
        {
            using (SqliteCommand cmd = this.SqliteConnection.CreateCommand())
            {
                cmd.CommandText = statement;
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteQuery(string statement)
        {
            try
            {
                using (SqliteCommand cmd = this.SqliteConnection.CreateCommand())
                {
                    cmd.CommandText = statement;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                Trace.WriteLine($"SQLiteException: { ex.Message }, ErrorCode: { ex.ErrorCode }, InnerException: {ex.InnerException} - SQL-Statement: { statement }");
                throw;
            }
        }

        public void SetPragmaValue(string pragmaName, string pragmaValue)
        {
            using (SqliteCommand cmd = this.SqliteConnection.CreateCommand())
            {
                cmd.CommandText = $"PRAGMA { pragmaName } = '{ pragmaValue }';";
                cmd.ExecuteNonQuery();

                // TODO: Check return code?!?
            }
        }

        private void CreateEntityModel(Type entityType)  //ILightedEntity enumeratedEntity)
        {

            // Define the table name for this Entity. If its type name ends with 'Entity', remove this suffix
            var tableName = entityType.Name;
            var entityIdx = tableName.LastIndexOf("Entity", StringComparison.OrdinalIgnoreCase);
            if (-1 != entityIdx)
                tableName = tableName.Remove(entityIdx);

            EntityStatementBuilder stmtBuild = new EntityStatementBuilder(tableName);

            foreach (var property in entityType.GetProperties())
            {
                // Iterate through all properties, looking for ColumnAttribute

                if (property.GetCustomAttribute(typeof(ColumnAttribute)) is ColumnAttribute serAttr)
                    stmtBuild.AddAttribute(property.Name, serAttr);
            }

            this.ExecuteNonQuery(stmtBuild.ToSQLStatement());








            /*
                    public string ColumnName { get; }
                    public DataType DataType { get; }

                    // Note that: https://stackoverflow.com/questions/1168535/when-is-a-custom-attributes-constructor-run
                    public SqliteColumnAttribute(string columnName, DataType dataType)
                    {
                        // TODO: Check if type of (this) is ILightedEntity; Constructor gets called when Entity is examined using reflection
                        //var typeOf = this.GetType();

                        this.ColumnName = columnName;
                        this.DataType = dataType;
                    }

                    // Named Arguments

                    public bool Nullable { get; set; } = true;
                    public string Default { get; set; }
            */





        }

        #region Session Entities ==============================================================================================

        // TODO: Extension-Methode => object result = sqlCmd.ExecuteScalar("{STMT}");

        // TODO: public long CreateNewEntity( ENUM { Session, Content, Workbench, etc.} !)

        // oder: public long CreateNewEntity(typeof(ElSessionEntity))

        ////////public long CreateNewEntityId(ElSessionEntity newSession)       ==> SubClassing (z.B. ElEntityWithID)  => const TableName = 'Session'; string NewItemName = 'Neue Session!" => SQL-Stmt dynamisch aufbauen!
        ////////{
        ////////    return (newSession.Id = this.CreateNewSessionId());
        ////////}



        internal long CreateNewEntity(ILightedEntity entity, SetEntityCreationParamsCallback setEntityCreationParams)
        {
            // TODO: NULL-Checks
            // TODO: Additional fields!
            // TODO: SQL-Injection Checks

            if (null == setEntityCreationParams)
                throw new ArgumentNullException(nameof(setEntityCreationParams));

            using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
            {
                setEntityCreationParams(sqlCmd);

                lock (this.databaseLock)
                {
                    if (1 == sqlCmd.ExecuteNonQuery())
                    {
                        sqlCmd.CommandText = $"SELECT Id FROM { entity.DatabaseTableName } WHERE RowId = Last_Insert_RowId()";

                        var entityId = sqlCmd.ExecuteScalar();

                        if (null != entityId)
                            return (long)entityId;
                    }

                    throw new Exception($"Failed to create new record in database for { entity.GetType().Name }!"); // TODO: Overhault exception handlers!
                }
            }
        }



        #endregion ============================================================================================================




        /*
                        // TODO: 29.03: Lock database

                        sqlCmd.CommandText = $"INSERT INTO Session (Name) VALUES ( 'electrifier session' );";
                        var rowCount = sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = $"SELECT last_insert_rowid();";
                        var rowid = sqlCmd.ExecuteScalar();

                        var rawrowid = SQLitePCL.raw.sqlite3_last_insert_rowid(entityStore.SqliteConnection.Handle);       // long
        */

        //private void LockDatabase()
        //{

        //}

        //public void UnlockDatabase()
        //{

        //}

        //public void BackupDatabaseToFile()
        //{
        //    // See https://docs.microsoft.com/de-de/dotnet/standard/data/sqlite/backup
        //    // Create a full backup of the database
        //    var backup = new SqliteConnection("Data Source=BackupSample.db");
        //    this.sqlConnection.BackupDatabase(backup);

        //    //var fullFileName = Path.Combine(this.ApplicationDataPath, this.ConfigurationFileName);

        //    //// Create directory for configuration file, just in case it doesn't already exist
        //    //Directory.CreateDirectory(this.ApplicationDataPath);

        //    //this.ElectrifierForm.SaveConfiguration(fullFileName);
        //}

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    if (this.SqliteConnection.State == System.Data.ConnectionState.Open)
                        this.SqliteConnection.Close();

                    this.SqliteConnection.Dispose();
                    this.SqliteConnection = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EntityLighter()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
