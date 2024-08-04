using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EntityLighter.Collections;
using EntityLighter.Storage;
using Microsoft.Data.Sqlite;


// TODO: There is a new MS-Project: https://github.com/linq2db/linq2db :(

/// <summary>
/// <seealso href="https://en.wikipedia.org/wiki/Entity–relationship_model"/>
/// <seealso href="https://en.wikipedia.org/wiki/Data_modeling"/>
/// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.mapping.tableattribute?view=netframework-4.8"/>
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

// This is how you could 'embed' C#-functions into SQLite!
// https://stackoverflow.com/questions/24229785/sqlite-net-sqlitefunction-not-working-in-linq-to-sql/26155359#26155359
// https://docs.microsoft.com/de-de/dotnet/standard/data/sqlite/user-defined-functions
// this.SqliteConnection.CreateFunction<>

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
    /// List of common SQLite ErrorCodes
    /// 
    /// See <seealso href="https://sqlite.org/rescode.html"/> for further information.
    /// 
    /// </summary>
    public enum SqliteResult
    {
        /// <summary>
        /// The SQLITE_OK result code means that the operation was successful and that there were no errors. Most other result codes indicate an error. 
        /// </summary>
        SQLITE_OK = 0,
        /// <summary>
        /// The SQLITE_ERROR result code is a generic error code that is used when no other more specific error code is available.
        /// </summary>
        SQLITE_ERROR = 1,
        /// <summary>
        /// The SQLITE_INTERNAL result code indicates an internal malfunction. In a working version of SQLite, an application should never see this result code. If application does encounter this result code, it shows that there is a bug in the database engine.
        /// SQLite does not currently generate this result code. However, application-defined SQL functions or virtual tables, or VFSes, or other extensions might cause this result code to be returned.
        /// </summary>
        SQLITE_INTERNAL = 2,
        /// <summary>
        /// The SQLITE_PERM result code indicates that the requested access mode for a newly created database could not be provided. 
        /// </summary>
        SQLITE_PERM = 3,
        /// <summary>
        /// The SQLITE_ABORT result code indicates that an operation was aborted prior to completion, usually be application request. See also: SQLITE_INTERRUPT.
        /// 
        /// If the callback function to sqlite3_exec() returns non-zero, then sqlite3_exec() will return SQLITE_ABORT.
        /// 
        /// If a ROLLBACK operation occurs on the same database connection as a pending read or write, then the pending read or write may fail with an SQLITE_ABORT or SQLITE_ABORT_ROLLBACK error.
        /// 
        /// In addition to being a result code, the SQLITE_ABORT value is also used as a conflict resolution mode returned from the sqlite3_vtab_on_conflict() interface.
        /// </summary>
        SQLITE_ABORT = 4,
        /// <summary>
        /// The SQLITE_BUSY result code indicates that the database file could not be written (or in some cases read) because of concurrent activity by some other database connection, usually a database connection in a separate process.
        /// 
        /// For example, if process A is in the middle of a large write transaction and at the same time process B attempts to start a new write transaction, process B will get back an SQLITE_BUSY result because SQLite only supports one writer at a time.Process B will need to wait for process A to finish its transaction before starting a new transaction.The sqlite3_busy_timeout() and sqlite3_busy_handler() interfaces and the busy_timeout pragma are available to process B to help it deal with SQLITE_BUSY errors.
        /// 
        /// An SQLITE_BUSY error can occur at any point in a transaction: when the transaction is first started, during any write or update operations, or when the transaction commits.To avoid encountering SQLITE_BUSY errors in the middle of a transaction, the application can use BEGIN IMMEDIATE instead of just BEGIN to start a transaction.The BEGIN IMMEDIATE command might itself return SQLITE_BUSY, but if it succeeds, then SQLite guarantees that no subsequent operations on the same database through the next COMMIT will return SQLITE_BUSY.
        /// 
        /// See also: SQLITE_BUSY_RECOVERY and SQLITE_BUSY_SNAPSHOT.
        /// 
        /// The SQLITE_BUSY result code differs from SQLITE_LOCKED in that SQLITE_BUSY indicates a conflict with a separate database connection, probably in a separate process, whereas SQLITE_LOCKED indicates a conflict within the same database connection (or sometimes a database connection with a shared cache).
        /// </summary>
        SQLITE_BUSY = 5,
        /// <summary>
        /// The SQLITE_LOCKED result code indicates that a write operation could not continue because of a conflict within the same database connection or a conflict with a different database connection that uses a shared cache.
        /// 
        /// For example, a DROP TABLE statement cannot be run while another thread is reading from that table on the same database connection because dropping the table would delete the table out from under the concurrent reader.
        /// 
        /// The SQLITE_LOCKED result code differs from SQLITE_BUSY in that SQLITE_LOCKED indicates a conflict on the same database connection (or on a connection with a shared cache) whereas SQLITE_BUSY indicates a conflict with a different database connection, probably in a different process.
        /// </summary>
        SQLITE_LOCKED = 6,

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



        public delegate void SetEntityCreationParamsCallback(SqliteCommand sqliteCommand);
        public delegate void SetEntityUpdateParamsCallback(SqliteCommand sqliteCommand);


        // TODO: Move types to AddEntityTable-Method!
        public DataContext(string storage, bool createIfNotExists = false)
        {
            this.Storage = storage ?? throw new ArgumentNullException(nameof(storage));


            SqliteResult returnCode = this.OpenDatabase(createIfNotExists); /* ThrowIfFailed */

            StorageModel storageModel = new StorageModel(this);

            //EntityBaseSet<EntityBase> test =
            //    Select<EntityBase>
            //    .LoadEntities()
            //    .AllRows(this, (sqlReader) => { return new EntityBase(sqlReader); });

            //            var test = storageModel.GetTableNames();


            //    /*
            //     * TEST PRAGMAS => Unit-Test
            //     */
            //    string userver = this.PragmaUserVersion;
            //    this.PragmaUserVersion.Value = "666";
            //    string userver2 = this.PragmaUserVersion;
            //    long vertest = (long)this.PragmaUserVersion;
            //    this.PragmaUserVersion.Value = userver;

            //    var fksupport = this.PragmaForeignKeys.Value;

            //    /*
            //                if (pragma.Value is long longResult)
            //    return longResult;
            //throw new InvalidCastException();

            //    */


            //    //this.SetPragmaValue("user_version", sqlUserVersionID.ToString(CultureInfo.InvariantCulture));


            //    //this.SetPragmaValue("foreign_keys", "ON");

            //    //                //foreach (var type in types)
            //    //                //    this.CreateEntityModel(type);
            //    //
            //    //this.ExecuteNonQuery(EntityLighter.SqlStmtCreateDatabase);



            //    //          // TODO: 17.04.2020!!! Fehlerhandling!
            //    //
            //    //
            //    //
            //    //                ElSessionEntity session = new ElSessionEntity(this);        // TODO: Exception: "Obsolete!" => Keine vernünftige Fehlermeldung!
            //    //
            //    //
            //    //              // throw new Exception("Ätsch!"); => Der Startcode fällt beim LADEN schon auf die Füsse, nicht beim Ausführen...
            //    //



            //    //                SessionEntity sessionEntity = new SessionEntity(this);

            //}
            //catch (SqliteException)
            //{
            //    //var ext = "SQL - Exception: " + ex.Message;

            //    //if (null != ex.InnerException)
            //    //    ext += "\n\nInner: " + ex.InnerException.Message;

            //    //System.Windows.Forms.MessageBox.Show(ext);
            //    throw;
            //}
        }

        /*
            public class EntitySet<TEntity>
              : IList<TEntity> where TEntity : class 
        */

        /* 
         * 
         * * * 
         */

        //public static EntitySet<TEntity> SelectEntities(string sqlStatement) where TEntity : class
        //{

        //}

        /*         public EntitySet<TEntity> Load(string commandText, LoadEntityCallback loadEntityCallback)
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
        }*/



        //public SelectFromTable(DataContext context, string sqlCommand)
        //{
        //    this.DataContext = context ?? throw new ArgumentNullException(nameof(context));
        //    this.CommandText = sqlCommand ?? throw new ArgumentNullException(nameof(sqlCommand));

        //    // TODO: Verify command here

        //    this.Command = this.DataContext.SqliteConnection.CreateCommand();
        //    Command.CommandText = sqlCommand;

        //    this.DataReader = this.Command.ExecuteReader();

        //}


        /// <summary>
        /// TODO: Move to StorageModel
        /// </summary>
        /// <param name="CreateIfNotExists"></param>
        protected SqliteResult OpenDatabase(bool CreateIfNotExists = false)
        {
            SqliteOpenMode openMode = SqliteOpenMode.ReadWriteCreate;
            int errorCode = 0;

            try
            {
                this.SqliteConnection = new SqliteConnection(
                    new SqliteConnectionStringBuilder()
                    {
                        DataSource = this.Storage,
                        Mode = openMode, //SqliteOpenMode.ReadWrite, //ReadWriteCreate,      //Mode = SqliteOpenMode.Memory,
                    }.ToString()
                );

                this.SqliteConnection.Open();

                //// Define PRAGMA helper objects // TODO: Use structs for this
                this.PragmaUserVersion = new SqlitePragma(this, "USER_VERSION");
                this.PragmaForeignKeys = new SqlitePragma(this, "FOREIGN_KEYS", isWriteable: false);
                this.PragmaApplicationID = new SqlitePragma(this, "APPLICATION_ID");
            }
            catch (SqliteException ex)
            {
                errorCode = ex.ErrorCode;
            }

            return (SqliteResult)errorCode;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns>The number of rows inserted, updated or delated. -1 for SELECT statements.</returns>
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

        //public SqliteCommand CreateCommand()
        //{
                // TODO: Use Fluent Interface for command creation and parameter binding
        //}

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






        public long CreateNewEntity(Type entityType, SetEntityCreationParamsCallback setEntityCreationParams)
        {
            // TODO: NULL-Checks
            // TODO: Additional fields!
            // TODO: SQL-Injection Checks
            // TODO: 24/05/20: Create the WHOLE statement dynamically!

            if (null == setEntityCreationParams)
                throw new ArgumentNullException(nameof(setEntityCreationParams));

            try
            {
                using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
                {
                    // TODO: 24/05/20: Create the WHOLE statement dynamically!; Use Tables attributes for dynamic binding
                    setEntityCreationParams(sqlCmd);

                    lock (this.databaseLock)
                    {
                        if (1 == sqlCmd.ExecuteNonQuery())
                        {
                            // Finally fetch the Primary key for the new record
                            sqlCmd.CommandText = $"SELECT Id FROM { EntityBase.GetStorageTable(entityType) } WHERE RowId = Last_Insert_RowId()";

                            object entityId = sqlCmd.ExecuteScalar();

                            if (null != entityId)
                                return (long)entityId;
                        }
                    }
                }

                throw new Exception($"Command creation failed."); // TODO: Overhault exception handlers!
            }
            catch (Exception innerException)
            {
                throw new Exception($"Failed to create new record of type { entityType.Name }!", innerException);
            }
        }


        public int UpdateEntity(Type entityType, SetEntityUpdateParamsCallback callback)
        {
            if (null == callback)
                throw new ArgumentNullException(nameof(callback));

            using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
            {
                // TODO: 24/05/20: Create the WHOLE statement dynamically!; Use Tables attributes for dynamic binding
                callback(sqlCmd);

                if (1 != sqlCmd.ExecuteNonQuery())
                    throw new Exception($"Failed to update record in database for { entityType }!"); // TODO: Overhault exception handlers!
            }

            return 1;
        }


        public static DateTime ConvertToDateTime(string datetimeString)
        {
            // TODO: Put this into an default type converting place SQLite <-> C#-Entities, see also ApplicationWindow.fspFormStatePersistor_LoadingFormState
            if (DateTime.TryParse(datetimeString, out DateTime result))
                return result;

            return default;
        }

        #region IDisposable Interface

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



    public class EntityLighterException : Exception
    {
        public SqliteCommand SqliteCommand { get; }
        public OriginOfException Origin { get; }

        public EntityLighterException(string message, SqliteException sqliteException,
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = -1)
            : base(new OriginOfException(message, filePath, memberName, lineNumber).ToString(), sqliteException)
        {
            this.Origin = new OriginOfException(message, filePath, memberName, lineNumber);
        }

        public EntityLighterException(string message, SqliteException sqliteException, SqliteCommand sqliteCommand)
            :base(message, sqliteException)
        {
            this.SqliteCommand = sqliteCommand;
        }

        public struct OriginOfException
        {
            public string Message { get; }
            public string CallerFilePath { get; }
            public string CallerMemberName { get; }
            public int CallerLineNumber { get; }
            public OriginOfException(
                String message,
                [System.Runtime.CompilerServices.CallerFilePath] string filePath = null,
                [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
                [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = -1)
            {
                this.Message = message;
                this.CallerFilePath = filePath;
                this.CallerMemberName = memberName;
                this.CallerLineNumber = lineNumber;
            }

            public override string ToString()
            {
                return ($"'{this.Message}'" +
                    (string.IsNullOrEmpty(this.CallerMemberName) ? null : $"\n  at {this.CallerMemberName}()") +
                    (string.IsNullOrEmpty(this.CallerFilePath) ? null : $"  in file '{this.CallerFilePath}'") +
                    ((this.CallerLineNumber > 0) ? $" at line #{this.CallerLineNumber}" : null));
            }
        }
    }
}
