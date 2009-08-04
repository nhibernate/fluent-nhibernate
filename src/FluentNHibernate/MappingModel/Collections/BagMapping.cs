using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Collections
{
    public class BagMapping : CollectionMappingBase
    {
        private readonly AttributeStore<BagMapping> attributes;

        public BagMapping()
            : this(new AttributeStore())
        {}

        public BagMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<BagMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessBag(this);
            base.AcceptVisitor(visitor);
        }

        public override string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<BagMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<BagMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<BagMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}