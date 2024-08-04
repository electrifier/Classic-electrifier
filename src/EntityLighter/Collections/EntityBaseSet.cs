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

namespace EntityLighter.Collections
{
    public class EntityBaseSet<TEntity> : IList<TEntity> where TEntity : EntityBase
    {
        /// <seealso href="https://books.google.de/books?id=VGnQPbG7vKwC&pg=PT127&lpg=PT127&dq=ilistsource&source=bl&ots=myckzCQmSW&sig=ACfU3U2RDvbZBJKgUC-SJWd-N0PUO0DTJw&hl=de&sa=X&ved=2ahUKEwiQisLlstzpAhWnUhUIHQ2WA2IQ6AEwCHoECAcQAQ#v=onepage&q=ilistsource&f=false">

        public DataContext DataContext { get; }
        protected ItemList<TEntity> entities;

        protected int version; // // TODO: Implement version counter

        public EntityBaseSet(DataContext dataContext)
        {
            this.DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        // TODO: Default, ohne CommandText => Return all entities from database
        // TODO: Implement Array Access overloading TEntity[]



        /// <summary>
        /// Ablauf: Alle [Columns].Namen dieser Klasse sammeln und daraus createstatement bauen!
        /// Columns sind EntityProperty<typeparamref name="TEntity"/>. Diese haben den StorageModelName.
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        // TODO: reimplement commandText as an where-Clause
        // var entities = new EntityBaseSet<Session>.Load(where.session = sessionid);
        //protected EntityBaseSet<TEntity> Load(string commandText, LoadEntityCallback callback)
        //{
        //    return this;
        //}
        //protected EntityBaseSet<TEntity> Load(Select<TEntity> query)
        //{
        //    return this ; // .where statement is true
        //}

        //public LoadEntityBaseSetStatement<TEntity> Where(string condition)
        //{
        //    //            return new LoadEntityBaseSetStatement<TEntity>().Where("id = 1");

        //    return null;

        //}

        #region IList<T>-Member ================================================================================================

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

        public int Count => this.entities.Count;

        bool ICollection<TEntity>.IsReadOnly => false;       // TODO: Implement writeable approach

        /// <summary>
        /// Interface: ICollection<TEntity>
        /// </summary>
        /// <returns></returns>
        public void Add(TEntity item)
        {
            this.entities.Add(item ?? throw new ArgumentNullException(nameof(item)));
        }

        void ICollection<TEntity>.Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TEntity item) => this.IndexOf(item) >= 0;

        void ICollection<TEntity>.CopyTo(TEntity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface: IEnumerable<TEntity>
        /// </summary>
        /// <returns></returns>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Interface: IEnumerable
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface: IList<TEntity>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface: IList<TEntity>
        /// </summary>
        /// <returns></returns>
        public void Insert(int index, TEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface: ICollection<TEntity>
        /// </summary>
        /// <returns></returns>
        public bool Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Interface: IList<TEntity>
        /// </summary>
        /// <returns></returns>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        class Enumerator : IEnumerator<TEntity>
        {
            public EntityBaseSet<TEntity> EntitySet { get; }
            public TEntity[] Items { get; }

            public int Index { get; set; }
            private int EndIndex { get; }
            private int Version { get; }        // TODO: Implement version!

            public Enumerator(EntityBaseSet<TEntity> entitySet)
            {
                this.EntitySet = entitySet;
                this.Items = entitySet.entities.Items;
                this.Index = -1;
                this.EndIndex = entitySet.entities.Count - 1;
                //this.version = entitySet.version;
            }

            public TEntity Current => this.Items[Index];

            object IEnumerator.Current => this.Items[Index];

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

            public void Reset()
            {
                //if (version != entitySet.version)
                //    throw new Exception("Entity Set modified while enumerating");

                this.Index = -1;
            }
        }
    }

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
            if (this.items == null || this.items.Length == this.Count)
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
            for (var i = 0; i < this.Count; i++)
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
            var i = this.Count;

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
            var i = this.IndexOf(item);
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
