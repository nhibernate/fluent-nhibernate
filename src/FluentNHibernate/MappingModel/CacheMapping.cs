using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class CacheMapping : MappingBase
    {
        readonly AttributeStore attributes;

        public CacheMapping()
            : this(new AttributeStore())
        {}

        public CacheMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCache(this);
        }

        public string Region
        {
            get { return attributes.GetOrDefault<string>("Region"); }
        }

        public string Usage
        {
            get { return attributes.GetOrDefault<string>("Usage"); }
        }

        public string Include
        {
            get { return attributes.GetOrDefault<string>("Include"); }
        }

        public Type ContainedEntityType { get; set; }

        public bool Equals(CacheMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) && Equals(other.ContainedEntityType, ContainedEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CacheMapping)) return false;
            return Equals((CacheMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((attributes != null ? attributes.GetHashCode() : 0) * 397) ^ (ContainedEntityType != null ? ContainedEntityType.GetHashCode() : 0);
            }
        }

        public void Set<T>(Expression<Func<CacheMapping, T>> expression, int layer, T value)
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