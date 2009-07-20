using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Identity
{
    public class KeyManyToOneMapping : MappingBase
    {
        private readonly AttributeStore<KeyManyToOneMapping> attributes = new AttributeStore<KeyManyToOneMapping>();
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKeyManyToOne(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public string NotFound
        {
            get { return attributes.Get(x => x.NotFound); }
            set { attributes.Set(x => x.NotFound, value); }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get {
                return columns;
            }
        }

        public Type ContainingEntityType { get; set; }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public bool IsSpecified<TResult>(Expression<Func<KeyManyToOneMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<KeyManyToOneMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<KeyManyToOneMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}