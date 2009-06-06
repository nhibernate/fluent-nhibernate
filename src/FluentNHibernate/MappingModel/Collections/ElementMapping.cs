using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Collections
{
    public class ElementMapping : MappingBase
    {
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();
        private readonly AttributeStore<ElementMapping> attributes = new AttributeStore<ElementMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessElement(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public AttributeStore<ElementMapping> Attributes
        {
            get { return attributes; }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }
    }
}