using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
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

        public bool IsSpecified<TResult>(Expression<Func<CacheMapping, TResult>> property)
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
    }
}