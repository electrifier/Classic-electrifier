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

using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EntityLighter
{
    [Table(Name = "EntityBase")]
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
    }


    public struct EntityProperty<T>
    {
        public T Value { get; }

        public bool IsLoaded
        {
            private set; get;
        }

        public static string StorageModelColumn { get; }

        public EntityProperty(string rowId)
        {
            this.Value = default(T);
            this.IsLoaded = false;
        }

        public static implicit operator T(EntityProperty<T> property) => property.Value;
    }

    /// <summary>
    /// 
    /// </summary>

    public class sqlsnippet
    {

    }



    

}
