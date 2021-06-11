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

using EntityLighter.Collections;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLighter.Queries
{
    /*
     * https://scottlilly.com/how-to-create-a-fluent-interface-in-c/#:~:text=%20How%20to%20create%20a%20fluent%20interface%20in,into%20how%20to%20add%20grammar%20to...%20More%20 
    */
    public interface ICanAddCondition<TEntity>
        where TEntity : EntityBase
    {
        ICanAddWhereValue<TEntity> Where(string columnName);
        EntityBaseSet<TEntity> AllRows(DataContext dataContext, Select<TEntity>.CreateEntityFromDataReader dataReader);
    }
    public interface ICanAddWhereValue<TEntity>
        where TEntity : EntityBase
    {
        ICanAddWhereOrRun<TEntity> IsEqual(object value);
        ICanAddWhereOrRun<TEntity> NotEqual(object value);
    }
    public interface ICanAddWhereOrRun<TEntity>
        where TEntity : EntityBase
    {
        ICanAddWhereValue<TEntity> Where(string columnName);
        EntityBaseSet<TEntity> RunNow(DataContext dataContext, Select<TEntity>.CreateEntityFromDataReader dataReader);
    }

    public class Select<TEntity>
        : ICanAddCondition<TEntity>
        , ICanAddWhereValue<TEntity>
        , ICanAddWhereOrRun<TEntity>
        where TEntity : EntityBase
    {
        private readonly string storageTableName;
        private readonly List<WhereCondition> whereConditions = new List<WhereCondition>();

        private string currentWhereConditionColumn;

        public delegate TEntity CreateEntityFromDataReader(SqliteDataReader reader);

        /// <summary>
        /// Private constructor to force object creation through fluent interface.
        /// 
        /// </summary>
        /// <param name="entityType">The entity type to select.</param>
        /// <exception cref="ArgumentException">Thrown when no table attribute could be found.</exception>
        private Select(Type entityType)
        {
            this.storageTableName = (Attribute.GetCustomAttribute(entityType, typeof(TableAttribute)) as TableAttribute).Name ?? typeof(TEntity).Name;
        }

        #region Initiating Method(s)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when no table attribute could be found.</exception>
        public static ICanAddCondition<TEntity> LoadEntities()
        {
            return new Select<TEntity>(typeof(TEntity));
        }

        #endregion

        #region Methods for chaining

        /// <summary>
        /// 
        /// <list type="bullet">
        /// <item>Interface <seealso cref="ICanAddWhereValue"/></item>
        /// </list>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ICanAddWhereOrRun</returns>
        public ICanAddWhereOrRun<TEntity> IsEqual(object value)
        {
            this.whereConditions.Add(new WhereCondition(this.currentWhereConditionColumn, WhereValue.isEqual, value));

            return this;
        }

        /// <summary>
        /// 
        /// <list type="bullet">
        /// <item>Interface <seealso cref="ICanAddWhereValue"/></item>
        /// </list>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ICanAddWhereOrRun</returns>
        public ICanAddWhereOrRun<TEntity> NotEqual(object value)
        {
            this.whereConditions.Add(new WhereCondition(this.currentWhereConditionColumn, WhereValue.isNotEqual, value));

            return this;
        }

        /// <summary>
        /// 
        /// <list type="bullet">
        /// <item>Interface <seealso cref="ICanAddCondition"/></item>
        /// <item>Interface <seealso cref="ICanAddWhereOrRun"/></item>
        /// </list>
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ICanAddWhereValue<TEntity> Where(string columnName)
        {
            this.currentWhereConditionColumn = columnName;
            return this;
        }

        #endregion

        #region Executing methods
        /// <summary>
        /// 
        /// <list type="bullet">
        /// <item>Interface <seealso cref="ICanAddCondition"/></item>
        /// </list>
        /// </summary>
        public EntityBaseSet<TEntity> AllRows(DataContext dataContext, CreateEntityFromDataReader dataReader)
        {
            try
            {
                return this.Execute(dataContext, dataReader);
            }
            catch (SqliteException ex)
            {
                throw new EntityLighterException($"Failed to fetch data from {this.storageTableName}", ex);
            }
        }

        /// <summary>
        /// 
        /// <list type="bullet">
        /// <item>Interface <seealso cref="ICanAddWhereOrRun"/></item>
        /// </list>
        /// </summary>
        public EntityBaseSet<TEntity> RunNow(DataContext dataContext, CreateEntityFromDataReader dataReader)
        {
            try
            {
                return this.Execute(dataContext, dataReader);
            }
            catch (SqliteException ex)
            {
                throw new EntityLighterException($"Failed to fetch data from {this.storageTableName}", ex);
            }
        }
        #endregion


        private EntityBaseSet<TEntity> Execute(DataContext dataContext, CreateEntityFromDataReader dataReader)
        {
            EntityBaseSet<TEntity> loadedItems = new EntityBaseSet<TEntity>(dataContext);
            SqliteCommand sqliteCommand;

            try
            {
                using (sqliteCommand = dataContext.SqliteConnection.CreateCommand())
                {
                    sqliteCommand.CommandText = this.PrepareSQLStatement();

                    using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
                    {
                        //loadedItems.Grow(reader => count); // TODO: Specify size here!

                        while (reader.Read())
                            loadedItems.Add(dataReader(reader));
                    }
                }
            }
            catch(SqliteException ex)
            {
                throw new EntityLighterException("Error while executing SQL-Query", ex);
            }

            return loadedItems;
        }

        private string PrepareSQLStatement()
        {
            StringBuilder queryBuilder = new StringBuilder($"SELECT * FROM { this.storageTableName }");

            // Append where conditions to selection
            if (this.whereConditions.Count > 0)
            {
                queryBuilder.Append(" WHERE "); // TODO: Have a query-Builder, which formats whitepaces accordingly!

                foreach (var condition in this.whereConditions)
                    queryBuilder.Append(condition.ToSQLSnippet());
            }

            return queryBuilder.ToString();
        }
    }

    public enum WhereValue
    {
        isEqual,
        isNotEqual
    }

    /* https://www.techonthenet.com/sqlite/comparison_operators.php */

    public static class WhereValueExtensions
    {
        public static string ToSQLSnippet(this WhereValue whereValue)
        {
            switch (whereValue)
            {
                case WhereValue.isEqual:
                    return "=";
                case WhereValue.isNotEqual:
                    return "!=";
            }

            throw new ArgumentOutOfRangeException(whereValue.ToString());
        }
    }

    public class WhereCondition
    {
        public string Column { get; }
        public WhereValue ComparisonMethod { get; }
        public string ComparisonValue { get; }
        public WhereCondition(string column, WhereValue comparisonMethod, object comparisonValue)
        {
            this.Column = column;
            this.ComparisonMethod = comparisonMethod;
            this.ComparisonValue = comparisonValue.ToString();        // TODO: Put converters here
        }

        public string ToSQLSnippet()
        {
            return $" { this.Column } { this.ComparisonMethod.ToSQLSnippet() } { this.ComparisonValue } ";
        }
    }
}
