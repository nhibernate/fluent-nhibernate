using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Collections
{
    public class ArrayMapping : CollectionMappingBase, IIndexedCollectionMapping
    {
        private readonly AttributeStore<ArrayMapping> attributes;

        public IIndexMapping Index
        {
            get { return attributes.Get(x => x.Index); }
            set { attributes.Set(x => x.Index, value); }
        }

        public ArrayMapping()
            : this(new AttributeStore())
        {}

        public ArrayMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<ArrayMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessArray(this);

            if (Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }

    	public override string OrderBy
    	{
			get { return null; }
    		set { /* no-op */  }
    	}

    	public bool IsSpecified<TResult>(Expression<Func<ArrayMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ArrayMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ArrayMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}