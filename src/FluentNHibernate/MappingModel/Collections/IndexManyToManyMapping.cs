using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public bool IsSpecified<TResult>(Expression<Func<IndexManyToManyMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<IndexManyToManyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<IndexManyToManyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}