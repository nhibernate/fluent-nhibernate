using System;
using System.Collections.Generic;

namespace ShadeTree.DomainModel.Mapping
{
    public static class ClassToTableMapping
    {
        private static readonly Dictionary<Type, string> _tables = new Dictionary<Type, string>();

        public static void TableFor<T>(string tableName)
        {
            if (_tables.ContainsKey(typeof (T)))
            {
                _tables[typeof (T)] = tableName;
            }
            else
            {
                _tables.Add(typeof (T), tableName);
            }
        }

        public static string GetTableNameFor<T>()
        {
            if (_tables.ContainsKey(typeof (T)))
            {
                return _tables[typeof (T)];
            }

            return typeof (T).Name;
        }

        public static void CopyTable<T1, T2>()
        {
            string tableName = GetTableNameFor<T1>();
            if (!_tables.ContainsKey(typeof (T2)))
            {
                _tables.Add(typeof (T2), tableName);
            }
        }
    }
}