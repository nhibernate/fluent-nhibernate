using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
    public class MetaValueMapping : MappingBase
    {
        private readonly AttributeStore<MetaValueMapping> attributes = new AttributeStore<MetaValueMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessMetaValue(this);
        }

        public string Value
        {
            get { return attributes.Get(x => x.Value); }
            set { attributes.Set(x => x.Value, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public Type ContainingEntityType { get; set; }

        public bool IsSpecified<TResult>(Expression<Func<MetaValueMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<MetaValueMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<MetaValueMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}