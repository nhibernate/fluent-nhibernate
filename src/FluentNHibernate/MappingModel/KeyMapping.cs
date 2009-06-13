using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class KeyMapping : MappingBase, IHasColumnMappings
    {
        private readonly AttributeStore<KeyMapping> attributes;
        private readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();
        public Type ContainedEntityType { get; set; }

        public KeyMapping()
        {
            attributes = new AttributeStore<KeyMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKey(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public AttributeStore<KeyMapping> Attributes
        {
            get { return attributes; }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyRef
        {
            get { return attributes.Get(x => x.PropertyRef); }
            set { attributes.Set(x => x.PropertyRef, value); }
        }

        public string OnDelete
        {
            get { return attributes.Get(x => x.OnDelete); }
            set { attributes.Set(x => x.OnDelete, value); }
        }

        public IDefaultableEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public void AddDefaultColumn(ColumnMapping mapping)
        {
            columns.AddDefault(mapping);
        }

        public void ClearColumns()
        {
            columns.Clear();
        }
    }
}