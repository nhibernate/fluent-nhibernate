using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentMapping : ComponentMappingBase
    {
        private AttributeStore<ComponentMapping> attributes = new AttributeStore<ComponentMapping>();

        public ComponentMapping()
            : this(new AttributeStore())
        {}

        public ComponentMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<ComponentMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }

        public override void MergeAttributes(AttributeStore store)
        {
            attributes.Merge(new AttributeStore<ComponentMapping>(store));
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

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<ComponentMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ComponentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ComponentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}