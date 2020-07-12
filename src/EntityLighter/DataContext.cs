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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Data.Sqlite;


// TODO: There is a new MS-Project: https://github.com/linq2db/linq2db :(

/// <summary>
/// <seealso href="https://en.wikipedia.org/wiki/Entity–relationship_model"/>
/// <seealso href="https://en.wikipedia.org/wiki/Data_modeling"/>
/// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.mapping.tableattribute?view=netframework-4.8"/>
/// 
/// The EntitySet-Class source:
/// <seealso href="https://referencesource.microsoft.com/#System.Data.Linq/Types.cs,dbc446811fd0241d"/>
/// </summary>


// TODO: 10/05/20: Use events for Error-Handling to enable easy overwriting
// TODO: 10/05/20: https://www.codeproject.com/Articles/43025/A-LINQ-Tutorial-Mapping-Tables-to-Objects
// TODO: Currently we don't support multi-column primary keys! https://www.sqlitetutorial.net/sqlite-primary-key/
// TODO: We may use INotifyPropertyChanged for updating in real-time
// TODO: See https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/attribute-based-mapping,
//           https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.mapping?view=netframework-4.8 for precise object names
// https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/the-linq-to-sql-object-model
// TODO: See https://docs.microsoft.com/en-us/visualstudio/test/unit-test-your-code?view=vs-2019 for Unit-Tests



// SQLite: Datetime: SELECT datetime('now','localtime');

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
    }


    /// <summary>
    /// TODO: Convert to class! Use overloads and operators heavily :)
    /// 
    /// <seealso href="https://sqlite.org/pragma.html"/>
    /// </summary>
    public struct SqlitePragma       // TODO: IEquatable
    {
        public DataContext DataContext { get; }
        public string Name { get; }
        public bool IsReadable { get; }
        public bool IsWriteable { get; }

        public SqlitePragma(DataContext dataContext, string name, bool isReadable = true, bool isWriteable = true)
        {
            this.DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.IsReadable = isReadable;
            this.IsWriteable = isWriteable;
        }

        public object Value
        {
            get
            {
                if (!this.IsReadable)
                    throw new AccessViolationException();

                return this.DataContext.ExecuteScalar($"PRAGMA { this.Name }");      // TODO: Convert <object> to long, real, blob, bool?
            }
            set
            {
                if (!this.IsWriteable)
                    throw new AccessViolationException();

                // TODO: Check ReturnCode!
                this.DataContext.ExecuteNonQuery($"PRAGMA { this.Name } = { value }");
            }
        }

        public static implicit operator string(SqlitePragma pragma) => pragma.Value.ToString();

        public static explicit operator long(SqlitePragma pragma)
        {
            if (pragma.Value is long longResult)
                return longResult;
            throw new InvalidCastException();
        }
    }

    #region Extensions ========================================================================================================

    internal static class Extensions
    {
        /// <summary>
        /// Convert <see cref="DataType"/> into SQL-Snippet
        /// </summary>
        /// <param name="dataType">The DataType of this table column</param>
        /// <returns></returns>
        public static string ToSQLSnippet(this DataType dataType)
        {
            return dataType.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// Convert <see cref="Constraint"/> into SQL-Snippet
        /// </summary>
        /// <param name="constraint">The Constraint flags for this table column</param>
        /// <returns></returns>
        public static string ToSQLSnippet(this Constraint constraint) =>
            $"{(constraint.HasFlag(Constraint.NotNull) ? "NOT NULL " : "")}" +
            $"{(constraint.HasFlag(Constraint.Unique) ? "UNIQUE" : "")}";
    }

    #endregion ================================================================================================================



    #region Attributes ========================================================================================================

    /// <summary>
    /// TableAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute() { }

        // Optional, Named Arguments
        public string Name { get; set; }
    }

    /// <summary>
    /// AssociationAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class AssociationAttribute
      : Attribute
    {
        public AssociationAttribute() { }

        // Optional, Named Arguments
        public string OtherKey { get; set; }
    }

    /// <summary>
    /// BaseColumnAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class BaseColumnAttribute
      : Attribute
    {
        public BaseColumnAttribute(DataType dataType)
        {
            this.DataType = dataType;
        }

        // Positional, Mandatory Argument
        public DataType DataType { get; }

        // Optional, Named Arguments
        public Constraint Constraints { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPrimaryKey { get; protected set; }

        /// <summary>
        /// Generate SQL-Snippet describing this property for use in CREATE TABLE-Statement
        /// </summary>
        /// <returns></returns>
        public string ToSQLSnippet(string columnName)
        {
            StringBuilder snippet = new StringBuilder($"{ columnName } { this.DataType.ToSQLSnippet() } { this.Constraints.ToSQLSnippet() } ");

            if (!string.IsNullOrWhiteSpace(this.DefaultValue))
                snippet.Append($"DEFAULT { this.DefaultValue }");

            return snippet.ToString();
        }
    }

    /// <summary>
    /// ColumnAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ColumnAttribute
      : BaseColumnAttribute
    {
        public ColumnAttribute(DataType dataType) : base(dataType)
        {
            this.IsPrimaryKey = false;
        }
    }

    /// <summary>
    /// PrimaryKeyAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class PrimaryKeyAttribute
      : BaseColumnAttribute
    {
        public PrimaryKeyAttribute(DataType dataType) : base(dataType)
        {
            this.IsPrimaryKey = true;
        }
    }


    #endregion ================================================================================================================

    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>

    public class DataContext
      : IDisposable
    {
        #region Fields ========================================================================================================

        private readonly object databaseLock = new object();

        #endregion ============================================================================================================

        #region Properties ====================================================================================================


        public string Storage { get; }
        public SqliteConnection SqliteConnection { get; private set; }

        public SqlitePragma PragmaForeignKeys;       // Implement GET/SET by Converting struct => class
        public SqlitePragma PragmaUserVersion;
        public SqlitePragma PragmaApplicationID;

        public bool HasForeignKeySupport { get => ((long)this.PragmaForeignKeys.Value == 1); }      // TODO: BOOL

        public string SQLiteVersion => this.ExecuteScalar("SELECT SQLite_Version()").ToString();
        public string SQLiteUserVersion
        {
            get => this.PragmaUserVersion;
            set => this.PragmaUserVersion.Value = value;
        }

        #endregion ============================================================================================================

        private class TableStatementBuilder
        {
            public string EntityName { get; }

            private readonly List<Tuple<string, BaseColumnAttribute>> TableColumns = new List<Tuple<string, BaseColumnAttribute>>();

            public TableStatementBuilder(string entityName)
            {
                this.EntityName = entityName;
            }

            public void AddColumn(string columnName, BaseColumnAttribute attributes)
            {
                this.TableColumns.Add(new Tuple<string, BaseColumnAttribute>(columnName, attributes));
            }

            public int AttributeCount => this.TableColumns.Count;

            public string ToSQLStatement()
            {
                int columnCount = this.TableColumns.Count;

                if (columnCount < 1)
                    return string.Empty;

                StringBuilder statement = new StringBuilder($"CREATE TABLE IF NOT EXISTS '{ this.EntityName }' ( ");
                List<string> primaryKey = new List<string>();

                // Add Column snippets
                for (int i = 0; i < columnCount;)
                {
                    this.TableColumns[i].Deconstruct(out string columnName, out BaseColumnAttribute attributes);

                    statement.Append(attributes.ToSQLSnippet(columnName));

                    if (attributes.IsPrimaryKey)
                        primaryKey.Add(columnName);

                    if (++i < columnCount)
                        statement.Append(", ");
                }

                // Add Primary Key snippet
                if (primaryKey.Count > 0)
                {
                    statement.Append(", PRIMARY KEY (");

                    for (int i = 0; i < primaryKey.Count;)
                    {
                        statement.Append(primaryKey[i]);
                        statement.Append(++i < primaryKey.Count ? ", " : ")");
                    }
                }

                statement.Append(")");
                return statement.ToString();
            }
        }



        // TODO: 10/05/20 Perhaps use an event for this?
        public delegate void SetEntityCreationParamsCallback(SqliteCommand sqliteCommand);



        public DataContext(string storage) // CreateIfNotExists-param // TODO: Move types to AddEntityTable-Method!
        {
            this.Storage = storage ?? throw new ArgumentNullException(nameof(storage));

            try
            {
                this.SqliteConnection = new SqliteConnection(
                    new SqliteConnectionStringBuilder()
                    {
                        DataSource = this.Storage,
                        Mode = SqliteOpenMode.ReadWrite, //ReadWriteCreate,      //Mode = SqliteOpenMode.Memory,
                    }.ToString()
                );

                // This is how you could 'embed' C#-functions into SQLite!
                // https://stackoverflow.com/questions/24229785/sqlite-net-sqlitefunction-not-working-in-linq-to-sql/26155359#26155359
                // https://docs.microsoft.com/de-de/dotnet/standard/data/sqlite/user-defined-functions
                //this.SqliteConnection.CreateFunction<>

                this.SqliteConnection.Open();

                // Define PRAGMA helper objects

                this.PragmaUserVersion = new SqlitePragma(this, "USER_VERSION");
                this.PragmaForeignKeys = new SqlitePragma(this, "FOREIGN_KEYS", isWriteable: false);
                this.PragmaApplicationID = new SqlitePragma(this, "APPLICATION_ID");

                // Check if Database model already exists
                using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
                {
                    // TODO: TEST-Code!
                    // TODO: Put into OnVerifyTableModel


                    sqlCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Session';"; // TODO: name in ('Session', 'Workbench', ... put all tables into it); AND verify UserVersionID!
                    var res = sqlCmd.ExecuteReader();

                    //if (res.HasRows)
                    //    System.Windows.Forms.MessageBox.Show("Database already defined!");
                }

                /*
                 * TEST PRAGMAS => Unit-Test
                 */
                string userver = this.PragmaUserVersion;
                this.PragmaUserVersion.Value = "666";
                string userver2 = this.PragmaUserVersion;
                long vertest = (long)this.PragmaUserVersion;
                this.PragmaUserVersion.Value = userver;

                var fksupport = this.PragmaForeignKeys.Value;

                /*
                            if (pragma.Value is long longResult)
                return longResult;
            throw new InvalidCastException();

                */


                //this.SetPragmaValue("user_version", sqlUserVersionID.ToString(CultureInfo.InvariantCulture));


                //this.SetPragmaValue("foreign_keys", "ON");

                //                //foreach (var type in types)
                //                //    this.CreateEntityModel(type);
                //
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

        public object ExecuteScalar(string statement)
        {
            using (SqliteCommand cmd = this.SqliteConnection.CreateCommand())
            {
                cmd.CommandText = statement;
                return cmd.ExecuteScalar();
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

        public bool TableExists(string tableName)
        {
            object queryResult = this.ExecuteScalar($"SELECT COUNT(*) FROM SQLite_Master WHERE Type='table' AND UPPER(Name)='{ tableName.ToUpperInvariant() }'");

            if (queryResult is long resint)
                return (1 == resint);

            return false;
        }

        public void CreateEntityModel(Type entityType)
        {
            if (!Attribute.IsDefined(entityType, typeof(TableAttribute)))
                throw new ArgumentException("No Table Attribute defined", nameof(entityType));

            TableAttribute table = Attribute.GetCustomAttribute(entityType, typeof(TableAttribute)) as TableAttribute;

            string tableName = table.Name ?? entityType.Name;

            if (this.TableExists(tableName))
            {
                // TODO: Compare table columns and update model if necessary
                return;
            }

            TableStatementBuilder tableStatement = new TableStatementBuilder(tableName);

            // Iterate through all properties and process ColumnAttributes
            foreach (var property in entityType.GetProperties())
            {
                if (property.GetCustomAttribute(typeof(BaseColumnAttribute)) is BaseColumnAttribute serAttr)
                    tableStatement.AddColumn(property.Name, serAttr);
            }

            if (tableStatement.AttributeCount > 0)
                this.ExecuteNonQuery(tableStatement.ToSQLStatement());
            else
                throw new MissingMemberException("No Column Attribute defined on any Property", nameof(entityType));
        }

        public string GetTableName(Type entityType)         // Extension-Method?!?
        {
            if (!Attribute.IsDefined(entityType, typeof(TableAttribute)))
                throw new ArgumentException("No Table Attribute defined", nameof(entityType));

            TableAttribute table = Attribute.GetCustomAttribute(entityType, typeof(TableAttribute)) as TableAttribute;

            return table.Name ?? entityType.Name;
        }



        public long CreateNewEntity(Type entityType, SetEntityCreationParamsCallback setEntityCreationParams)
        {
            // TODO: NULL-Checks
            // TODO: Additional fields!
            // TODO: SQL-Injection Checks
            // TODO: 24/05/20: Create the WHOLE statement dynamically!

            if (null == setEntityCreationParams)
                throw new ArgumentNullException(nameof(setEntityCreationParams));

            using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
            {
                // TODO: 24/05/20: Create the WHOLE statement dynamically!; Use Tables attributes for dynamic binding
                setEntityCreationParams(sqlCmd);

                lock (this.databaseLock)
                {
                    if (1 == sqlCmd.ExecuteNonQuery())
                    {
                        // Finally fetch the Primary key of the new record
                        sqlCmd.CommandText = $"SELECT Id FROM { this.GetTableName(entityType) } WHERE RowId = Last_Insert_RowId()";

                        var entityId = sqlCmd.ExecuteScalar();

                        if (null != entityId)
                            return (long)entityId;
                    }

                    throw new Exception($"Failed to create new record in database for { entityType }!"); // TODO: Overhault exception handlers!
                }
            }
        }





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

    // ========================================================================================================================
    //
    #region CLASS: EntitySet<TEntity> =========================================================================================
    //
    // ========================================================================================================================


    /// <summary>
    /// 
    /// For IListSource, see:
    /// <seealso href="https://books.google.de/books?id=VGnQPbG7vKwC&pg=PT127&lpg=PT127&dq=ilistsource&source=bl&ots=myckzCQmSW&sig=ACfU3U2RDvbZBJKgUC-SJWd-N0PUO0DTJw&hl=de&sa=X&ved=2ahUKEwiQisLlstzpAhWnUhUIHQ2WA2IQ6AEwCHoECAcQAQ#v=onepage&q=ilistsource&f=false"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntitySet<TEntity>
      : IList<TEntity> where TEntity : class
    {
        public DataContext DataContext { get; }
        protected ItemList<TEntity> entities;
        //int version;      // TODO: Implement version counter

        public EntitySet(DataContext dataContext)
        {
            this.DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        TEntity IList<TEntity>.this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.entities.Count))
                    throw new ArgumentOutOfRangeException(nameof(index));

                return this.entities[index];
            }

            set => throw new NotImplementedException();
        }

        #region ICollection-Interface

        public int Count => this.entities.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            this.entities.Add(item);
        }

        public bool Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TEntity item) => this.IndexOf(item) >= 0;

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            if (this.entities.Count > 0)
                Array.Copy(this.entities.Items, 0, array, arrayIndex, this.entities.Count);
        }

        #endregion

        #region IEnumerable-Interface

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IList<TEntity>-Interface

        public int IndexOf(TEntity item) => this.entities.IndexOf(item);

        public void Insert(int index, TEntity item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        public delegate TEntity LoadEntityCallback(SqliteDataReader sqliteDataReader);

        public EntitySet<TEntity> Load(string commandText, LoadEntityCallback loadEntityCallback)
        {
            ItemList<TEntity> loadedItems = new ItemList<TEntity>();

            using (SqliteCommand cmd = this.DataContext.SqliteConnection.CreateCommand())
            {
                cmd.CommandText = commandText;

                using (SqliteDataReader dataReader = cmd.ExecuteReader())
                {
                    // NOTE: Removed "if (dataReader.HasRows)"
                    while (dataReader.Read())
                        loadedItems.Add(loadEntityCallback(dataReader));
                }
            }

            // Finally, replace our Entity List with the loaded one
            // TODO: Consider disposing the old list
            this.entities = loadedItems;

            return this;        // TODO: Return the instance for for LINQ-statement concatenation, right?
        }

        /// <summary>
        /// Subclass EntitySet<TEntity>.Enumerator
        /// </summary>
        class Enumerator : IEnumerator<TEntity>
        {
            public EntitySet<TEntity> EntitySet { get; }
            public TEntity[] Items { get; }

            public int Index { get; set; }
            private int EndIndex { get; }
            private int Version { get; }

            public Enumerator(EntitySet<TEntity> entitySet)
            {
                this.EntitySet = entitySet;
                this.Items = entitySet.entities.Items;
                this.Index = -1;
                this.EndIndex = entitySet.entities.Count - 1;
                //this.version = entitySet.version;
            }

            public void Dispose() => GC.SuppressFinalize(this);

            public bool MoveNext()
            {
                //if (version != entitySet.version)
                //    throw new Exception("Entity Set modified while enumerating");

                if (this.Index == this.EndIndex)
                    return false;

                this.Index++;
                return true;
            }

            public TEntity Current => this.Items[Index];

            object IEnumerator.Current => this.Items[Index];

            void IEnumerator.Reset()
            {
                //if (version != entitySet.version)
                //    throw new Exception("Entity Set modified while enumerating");

                this.Index = -1;
            }
        }
    }

    #endregion CLASS: EntitySet<TEntity> ======================================================================================

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ItemList<T> where T : class
    {
        T[] items;

        public int Count { get; private set; }

        public T[] Items => this.items;

        public T this[int index]
        {
            get { return this.items[index]; }
            //set { this.items[index] = value; }
        }

        public void Add(T item)
        {
            if (this.items == null || this.items.Length == Count)
                this.GrowItems();

            this.items[this.Count] = item;
            this.Count++;
        }

        public bool Contains(T item) => this.IndexOf(item) >= 0;

        public Enumerator GetEnumerator() => new Enumerator(this.items, endIndex: this.Count - 1);

        public bool Include(T item)
        {
            if (this.LastIndexOf(item) >= 0)
                return false;

            this.Add(item);
            return true;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[i] == item)
                    return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (this.items == null || this.items.Length == this.Count)
                this.GrowItems();

            if (index < this.Count)
                Array.Copy(this.items, index, items, index + 1, Count - index);

            this.items[index] = item;
            this.Count++;
        }

        public int LastIndexOf(T item)
        {
            int i = this.Count;

            while (i > 0)
            {
                --i;
                if (this.items[i] == item)
                    return i;
            }

            return -1;
        }

        public bool Remove(T item)
        {
            int i = this.IndexOf(item);
            if (i < 0)
                return false;

            this.RemoveAt(i);
            return true;
        }

        public void RemoveAt(int index)
        {
            this.Count--;
            if (index < this.Count)
                Array.Copy(this.items, index + 1, this.items, index, this.Count - index);
            this.items[this.Count] = default;
        }

        void GrowItems() => Array.Resize(ref this.items, this.Count == 0 ? 4 : this.Count * 2);

        public struct Enumerator
        {
            private readonly T[] items;
            private readonly int endIndex;
            private int index;

            public Enumerator(T[] items, int endIndex, int startIndex = -1)
            {
                this.items = items;
                this.index = startIndex;
                this.endIndex = endIndex;
            }

            public bool MoveNext()
            {
                if (this.index == this.endIndex)
                    return false;

                this.index++;
                return true;
            }

            public T Current => this.items[index];
        }
    }
}
