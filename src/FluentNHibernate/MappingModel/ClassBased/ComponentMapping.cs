using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    [Serializable]
    public class ComponentMapping : ComponentMappingBase, IComponentMapping
    {
        public ComponentType ComponentType { get; set; }
        readonly AttributeStore attributes;

        public ComponentMapping(ComponentType componentType)
            : this(componentType, new AttributeStore())
        {}

        public ComponentMapping(ComponentType componentType, AttributeStore attributes)
            : base(attributes)
        {
            ComponentType = componentType;
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }

        public bool HasColumnPrefix
        {
            get { return !string.IsNullOrEmpty(ColumnPrefix); }
        }

        public string ColumnPrefix { get; set; }

        public override string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public override Type Type
        {
            get { return attributes.GetOrDefault<Type>("Type"); }
        }

        public TypeReference Class
        {
            get { return attributes.GetOrDefault<TypeReference>("Class"); }
        }

        public bool Lazy
        {
            get { return attributes.GetOrDefault<bool>("Lazy"); }
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

        public void Set<T>(Expression<Func<ComponentMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}