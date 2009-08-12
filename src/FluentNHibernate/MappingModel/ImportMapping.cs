using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
    public class ImportMapping : MappingBase
    {
        private readonly AttributeStore<ImportMapping> attributes = new AttributeStore<ImportMapping>();

        public ImportMapping()
            : this(new AttributeStore())
        {}

        public ImportMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ImportMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessImport(this);
        }

        public string Rename
        {
            get { return attributes.Get(x => x.Rename); }
            set { attributes.Set(x => x.Rename, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<ImportMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ImportMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ImportMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}