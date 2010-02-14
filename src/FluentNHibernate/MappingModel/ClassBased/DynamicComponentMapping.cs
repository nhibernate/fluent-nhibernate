using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Visitors;

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

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<DynamicComponentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public override bool HasValue(string property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<DynamicComponentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(DynamicComponentMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as DynamicComponentMapping);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                {
                    return (base.GetHashCode() * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                }
            }
        }
    }
}