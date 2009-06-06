using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Collections
{
    public class IndexMapping : MappingBase, IIndexMapping
    {
        private readonly AttributeStore<IndexMapping> attributes;
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public IndexMapping()
        {
            attributes = new AttributeStore<IndexMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIndex(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public AttributeStore<IndexMapping> Attributes
        {
            get { return attributes; }
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }
    }
}
