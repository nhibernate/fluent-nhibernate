using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class MetaValueMapping : MappingBase
    {
        private readonly AttributeStore<MetaValueMapping> attributes;

        public MetaValueMapping()
            : this(new AttributeStore())
        {}

        protected MetaValueMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<MetaValueMapping>(underlyingStore);
        }

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

        public override bool IsSpecified(string property)
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

        public bool Equals(MetaValueMapping other)
        {
            return Equals(other.attributes, attributes) && Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(MetaValueMapping)) return false;
            return Equals((MetaValueMapping)obj);
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