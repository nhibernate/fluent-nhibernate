using System;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ColumnMappingCollection
    {
        readonly IHasColumnMappings mapping;
        readonly AttributeStore sharedAttributes;

        public ColumnMappingCollection(IHasColumnMappings mapping, AttributeStore sharedAttributes)
        {
            this.mapping = mapping;
            this.sharedAttributes = sharedAttributes;
        }

        public void Add(string name)
        {
            mapping.AddColumn(new ColumnMapping(sharedAttributes) { Name = name });
        }

        public void Add(params string[] names)
        {
            foreach (var name in names)
            {
                Add(name);
            }
        }

        public void Add(string columnName, Action<ColumnPart> customColumnMapping)
        {
            var column = new ColumnMapping(sharedAttributes) { Name = columnName };
            var part = new ColumnPart(column);
            customColumnMapping(part);
            mapping.AddColumn(column);
        }

        public void Add(ColumnMapping column)
        {
            mapping.AddColumn(column);
        }

        public void Clear()
        {
            mapping.ClearColumns();
        }

        public int Count
        {
            get { return mapping.Columns.Count(); }
        }
    }

    public class ColumnMappingCollection<TParent>
    {
        private readonly TParent parent;
        readonly ColumnMappingCollection collection;

        public ColumnMappingCollection(TParent parent, IHasColumnMappings mapping, AttributeStore sharedAttributes)
            : this(parent, new ColumnMappingCollection(mapping, sharedAttributes))
        {}

        public ColumnMappingCollection(TParent parent, ColumnMappingCollection collection)
        {
            this.parent = parent;
            this.collection = collection;
        }

        public TParent Add(string name)
        {
            collection.Add(name);
            return parent;
        }

        public TParent Add(params string[] names)
        {
            collection.Add(names);
            return parent;
        }

        public TParent Add(string columnName, Action<ColumnPart> customColumnMapping)
        {
            collection.Add(columnName, customColumnMapping);
            return parent;
        }

        public TParent Add(ColumnMapping column)
        {
            collection.Add(column);
            return parent;
        }

        public TParent Clear()
        {
            collection.Clear();
            return parent;
        }

        public int Count
        {
            get { return collection.Count; }
        }
    }
}