using EntityLighter;
using Microsoft.Data.Sqlite;
using System;
using System.Globalization;

namespace electrifier.Core
{
    [Table(Name = "Configuration")]
    public class ConfigurationEntity
        : IEquatable<ConfigurationEntity>
    {
        public DataContext DataContext { get; }

        [PrimaryKey(DataType.Text)]
        public string Name { get; }

        [Column(DataType.Text)]
        public object Value
        {
            get
            {
                var result = this.DataContext.ExecuteScalar($"SELECT Value FROM Configuration WHERE Name = '{ this.Name }'");

                return (result == DBNull.Value ? null : result);
            }
            set
            {
                // TODO: when set to null, delete the entity from database?
                using (var sqlCmd = this.DataContext.SqliteConnection.CreateCommand())            // TODO: add where rowcount = 1?!?
                {
                    sqlCmd.CommandText = $"UPDATE Configuration SET Value = $Value WHERE Name = $Name";
                    sqlCmd.Parameters.AddWithValue("$Value", value);
                    sqlCmd.Parameters.AddWithValue("$Name", this.Name);

                    var rowCount = sqlCmd.ExecuteNonQuery();

                    if (0 == rowCount)
                    {
                        sqlCmd.CommandText = $"INSERT INTO Configuration (Name, Value) VALUES ($Name, $Value)";

                        rowCount = sqlCmd.ExecuteNonQuery();
                        if (1 == rowCount)
                            return;
                        else
                            throw new Exception("Insert failed");
                        // TODO: new InsertFailedException("Table: Configuration", "SQL-Statement", ")
                    }
                    else if (1 < rowCount)
                    {
                        throw new Exception("Unique contraint"); // TODO: UniqueConstraintException()
                        // Rollback?!? => Transaction
                    }
                }
            }
        }

        public ConfigurationEntity(DataContext DataContext, string Name)
        {
            this.DataContext = DataContext ?? throw new ArgumentNullException(nameof(DataContext));
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));


        }

        public bool IsNull { get => this.Value is null; }

        public override string ToString() => this.Value as string;
        public static implicit operator string(ConfigurationEntity configurationEntity)
            => configurationEntity?.ToString();

        public int ToInt32() => Convert.ToInt32(this.Value, CultureInfo.InvariantCulture);
        public static explicit operator int(ConfigurationEntity configurationEntity)
            => (configurationEntity ?? throw new ArgumentNullException(nameof(configurationEntity))).ToInt32();

        bool IEquatable<ConfigurationEntity>.Equals(ConfigurationEntity other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return (this.Name.Equals(other.Name) && this.Value.Equals(other.Value));
        }

        public override bool Equals(object obj) => ((IEquatable<ConfigurationEntity>)this).Equals(obj as ConfigurationEntity);

        public override int GetHashCode() => this.Name.GetHashCode();
    }

    #region = GlobalConfig =====================================================================================================

    public class GlobalConfig
    {
        public DataContext DataContext { get; }
        public ConfigurationEntity DefaultSession { get; }

        public GlobalConfig(DataContext dataContext)
        {
            this.DataContext = dataContext ??
                throw new ArgumentNullException(nameof(dataContext));

            this.DefaultSession = new ConfigurationEntity(dataContext, "Default Session");
        }
    }

    #endregion
}
