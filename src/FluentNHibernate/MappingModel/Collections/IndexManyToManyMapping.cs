using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Collections
{
    public class IndexManyToManyMapping : MappingBase, IIndexMapping
    {
        private readonly AttributeStore<IndexManyToManyMapping> attributes;
        private readonly IDefaultableList<ColumnMapping> columns = new DefaultableList<ColumnMapping>();

        public IndexManyToManyMapping()
        {
            attributes = new AttributeStore<IndexManyToManyMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIndex(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public AttributeStore<IndexManyToManyMapping> Attributes
        {
            get { return attributes; }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public IDefaultableEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }
    }
}