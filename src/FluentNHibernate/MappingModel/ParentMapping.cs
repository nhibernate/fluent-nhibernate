using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class ParentMapping : MappingBase
    {
        private readonly AttributeStore<ParentMapping> attributes;

        public ParentMapping()
            : this(new AttributeStore())
        {}

        protected ParentMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ParentMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessParent(this);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public Type ContainingEntityType { get; set; }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ParentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ParentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ParentMapping)) return false;
            
            return Equals((ParentMapping)obj);
        }

        public bool Equals(ParentMapping other)
        {
            return Equals(other.attributes, attributes) && Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((attributes != null ? attributes.GetHashCode() : 0) * 397) ^
                    (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
            }
        }
    }
}