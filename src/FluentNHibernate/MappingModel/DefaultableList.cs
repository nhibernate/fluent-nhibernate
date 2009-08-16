using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class DefaultableList<T> : IDefaultableList<T>
    {
        private readonly IList<T> userDefined = new List<T>();
        private readonly IList<T> defaults = new List<T>();

        public DefaultableList()
        {}

        public DefaultableList(IEnumerable<T> enumerable)
        {
            userDefined = new List<T>(enumerable);
        }

        #region List

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (userDefined.Count == 0)
                return defaults.GetEnumerator();

            return userDefined.GetEnumerator();
        }

        public void AddDefault(T item)
        {
            defaults.Add(item);
        }

        public void Add(T item)
        {
            defaults.Clear();
            userDefined.Add(item);
        }

        public void Clear()
        {
            userDefined.Clear();
        }

        public void ClearDefaults()
        {
            defaults.Clear();
        }

        public void ClearAll()
        {
            defaults.Clear();
            userDefined.Clear();
        }

        public bool Contains(T item)
        {
            return userDefined.Contains(item);
        }

        public bool ContainsDefault(T item)
        {
            return defaults.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            userDefined.CopyTo(array, arrayIndex);
        }

        public void CopyDefaultsTo(T[] array, int arrayIndex)
        {
            defaults.CopyTo(array, arrayIndex);
        }

        public void CopyAllTo(T[] array, int arrayIndex)
        {
            var combined = new List<T>(defaults);

            combined.AddRange(userDefined);

            combined.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return userDefined.Remove(item);
        }

        public bool RemoveDefault(T item)
        {
            return defaults.Remove(item);
        }

        public int Count
        {
            get
            {
                if (userDefined.Count == 0)
                    return defaults.Count;

                return userDefined.Count;
            }
        }

        public int CountDefaults
        {
            get { return defaults.Count; }
        }

        public int CountAll
        {
            get { return Count + CountDefaults; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            return userDefined.IndexOf(item);
        }

        public int IndexOfDefault(T item)
        {
            return defaults.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            userDefined.Insert(index, item);
        }

        public void InsertDefault(int index, T item)
        {
            defaults.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            userDefined.RemoveAt(index);
        }

        public void RemoveDefaultAt(int index)
        {
            defaults.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                if (userDefined.Count == 0)
                    return defaults[index];

                return userDefined[index];
            }
            set { userDefined[index] = value; }
        }

        #endregion

        public IEnumerable<T> Defaults
        {
            get { return defaults; }
        }

        public IEnumerable<T> UserDefined
        {
            get { return userDefined; }
        }
    }
}