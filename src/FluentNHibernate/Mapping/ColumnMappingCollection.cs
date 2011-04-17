using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ColumnMappingCollection<TParent> : IEnumerable<ColumnMapping>
    {
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();
        private readonly TParent parent;

        public ColumnMappingCollection(TParent parent)
        {
            this.parent = parent;
        }

        public TParent Add(string name)
        {
            var mapping = new ColumnMapping();
            mapping.Set(x => x.Name, Layer.UserSupplied, name);
            columns.Add(mapping);
            return parent;
        }

        public TParent Add(params string[] names)
        {
            foreach (var name in names)
            {
                Add(name);
            }
            return parent;
        }

        public TParent Add(string columnName, Action<ColumnPart> customColumnMapping)
        {
            var mapping = new ColumnMapping();
            mapping.Set(x => x.Name, Layer.UserSupplied, columnName);
            var part = new ColumnPart(mapping);
            customColumnMapping(part);
            columns.Add(mapping);
            return parent;
        }

        public TParent Add(ColumnMapping column)
        {
            columns.Add(column);
            return parent;
        }

        public TParent Clear()
        {
            columns.Clear();
            return parent;
        }

        public int Count
        {
            get { return columns.Count; }
        }

        public IEnumerator<ColumnMapping> GetEnumerator()
        {
            return columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}