using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class DynamicComponentMapping : ComponentMappingBase
    {
        private readonly AttributeStore<DynamicComponentMapping> attributes;

        public DynamicComponentMapping()
            : this(new AttributeStore())
        { }

        private DynamicComponentMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<DynamicComponentMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }

        public bool IsSpecified<TResult>(Expression<Func<DynamicComponentMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<DynamicComponentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<DynamicComponentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}