using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentMapping : ComponentMappingBase, IComponentMapping
    {
        public ComponentType ComponentType { get; set; }
        private readonly AttributeStore<ComponentMapping> attributes = new AttributeStore<ComponentMapping>();

        public ComponentMapping(ComponentType componentType)
            : this(componentType, new AttributeStore())
        {}

        public ComponentMapping(ComponentType componentType, AttributeStore store)
            : base(store)
        {
            ComponentType = componentType;
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

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ComponentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public override bool HasValue(string property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ComponentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(ComponentMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) &&
                Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ComponentMapping);
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