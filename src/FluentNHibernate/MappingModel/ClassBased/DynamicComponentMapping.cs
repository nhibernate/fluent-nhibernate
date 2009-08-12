using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class DynamicComponentMapping : ComponentMappingBase
    {
        private AttributeStore<DynamicComponentMapping> attributes;

        public DynamicComponentMapping()
            : this(new AttributeStore())
        { }

        public DynamicComponentMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<DynamicComponentMapping>(store);
        }

        public override string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public override Type Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }

        public override void MergeAttributes(AttributeStore store)
        {
            attributes.Merge(new AttributeStore<DynamicComponentMapping>(store));
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