using System;

namespace EntityLighter.Storage
{
    /// <summary>
    /// Class StorageModel will contain all the methods to handle the database (file)
    /// </summary>
    public class StorageModel
    {
        public bool IsValid { get; private set; } = false;
        public DataContext DataContext { get; private set; }

        public StorageModel(DataContext context)
        {
            this.DataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public StorageModel(string fileName, bool CreateIfNotExists = false)
        {
            this.DataContext = new DataContext(fileName, CreateIfNotExists);
        }




        //public EntitySet<T> Load() where T is class
        //{

        //}

        /*
                public EntitySet<TEntity> Load(string commandText, LoadEntityCallback loadEntityCallback)

        */


        //        public object[] GetTableNames()
        //        {
        //            EntityLighterCommand command = new EntityLighterCommand(this.DataContext, "SELECT tbl_Name FROM SQLite_Master WHERE Type = 'table'");

        //            var results = new List<object>();


        //            //            var result = new object[1];
        //            //         public bool FetchResult<T>(ref List<T> result)

        ////            singleFetch = new List<string>();

        //            if (0 < command.FetchResults(ref results))
        //            {
        //                var x = results;
        //                //                results.Add(singleFetch[0]);
        //                    //TraceDebug(results);
        //            }


        //            //var reuslt = command.FetchResult(ref singleFetch);


        //            //            object test = this.DataContext.Fetch($"SELECT tbl_Name FROM SQLite_Master WHERE Type = 'table'");

        //            //            EntityLighterQuery query = new EntityLighterQuery(this.DataContext, $"SELECT Name FROM SQLite_Master WHERE Type = 'table'");
        //            /*
        //                        public List<object> Fetch(string sqlStatement)
        //                        {
        //                            List<object> result = new List<object>();

        //                            // TODO: EntityLighterCommand(string sqlCommand, resultDelegate => );

        //                            using (SqliteCommand sqlCmd = this.SqliteConnection.CreateCommand())
        //                            {
        //                                sqlCmd.CommandText = sqlStatement;

        //                                DbDataReader dbReader = sqlCmd.ExecuteReader();

        //                                if (dbReader.HasRows)
        //                                {
        //                                    var test = dbReader.GetString(0);
        //                                    while (dbReader.NextResult())
        //                                    {
        //                                        result.Add(dbReader.GetValue(1));
        //                                    }
        //                                }
        //                            }

        //                            return result;
        //                        }
        //            */


        //            //            object res = this.DataContext.ExecuteScalar($"SELECT Name FROM SQLite_Master WHERE Type = 'table'");

        //            /*
        //             *             object queryResult = this.ExecuteScalar($"SELECT COUNT(*) FROM SQLite_Master WHERE Type='table' AND UPPER(Name)='{ tableName.ToUpperInvariant() }'");

        //            */
        //            return results.ToArray();
        //        }


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
}
