using Microsoft.Data.Sqlite;
using System;

namespace EntityLighter
{
    public class EntityBase
    {
        [PrimaryKey(DataType.Integer)]
        public long Id { get; protected set; }

        //public string rowId { get; }

        public EntityBase()
        {
        }

        public EntityBase(SqliteDataReader dataReader)
        {
            this.Id = (long)dataReader["Id"];
        }

        /// <summary>
        /// Public static method to get the name of the table storage that is used for
        /// persistence of the given <paramref name="entityType"/> in the storage model.
        /// <br/>
        /// This is done by extracting the given type's custom <see cref="TableAttribute"/>.
        /// When there is no optional <c>Name</c> argument provided by this <see cref="Type"/>'s
        /// <see cref="TableAttribute"/>, the <c>Name</c> of the given <paramref name="entityType"/> itself is returned.
        /// </summary>
        /// <param name="entityType">The type of entity to determine the table storage for.</param>
        /// <returns></returns>
        /// <example>
        /// The code below shows how to extract the table storage's name for <see cref="Type"/><c>entityType</c>:
        /// <code>EntityBase.GetStorageTable(entityType);</code>
        /// </example>
        /// <exception cref="ArgumentException">Thrown when no table attribute could be found.</exception>
        public static string GetStorageTable(Type entityType)
        {
            if (!Attribute.IsDefined(entityType, typeof(TableAttribute)))
                throw new ArgumentException($"No Table Attribute defined for { entityType }");

            TableAttribute tableAttribute = Attribute.GetCustomAttribute(entityType, typeof(TableAttribute)) as TableAttribute;

            return string.IsNullOrEmpty(tableAttribute.Name) ? entityType.Name : tableAttribute.Name;
        }
    }

    // TODO: Implement EntityProperty<T> like this:
    //public struct EntityProperty<T>
    //{
    //    public T Value { get; }

    //    public bool IsLoaded
    //    {
    //        private set; get;
    //    }

    //    public static string StorageModelColumn { get; }

    //    public EntityProperty(string rowId)
    //    {
    //        this.Value = default(T);
    //        this.IsLoaded = false;
    //    }

    //    public static implicit operator T(EntityProperty<T> property) => property.Value;
    //}

    // TODO: Implement SQLSnippet
    //public class SQLSnippet
    //{

    //}
}
