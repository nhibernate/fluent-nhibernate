using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class CacheMapping : MappingBase
    {
        private readonly AttributeStore<CacheMapping> attributes;

        public CacheMapping()
            : this(new AttributeStore())
        {}

        public CacheMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<CacheMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCache(this);
        }

        public string Region
        {
            get { return attributes.Get(x => x.Region); }
            set { attributes.Set(x => x.Region, value); }
        }

        public string Usage
        {
            get { return attributes.Get(x => x.Usage); }
            set { attributes.Set(x => x.Usage, value); }
        }

        public string Include
        {
            get { return attributes.Get(x => x.Include); }
            set { attributes.Set(x => x.Include, value); }
        }

        public Type ContainedEntityType { get; set; }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<CacheMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<CacheMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

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
    }
}