using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Collections
{
    public class MapMapping : CollectionMappingBase, IIndexedCollectionMapping
    {
        private readonly AttributeStore<MapMapping> attributes;
        public IIndexMapping Index
        {
            get { return attributes.Get(x => x.Index); }
            set { attributes.Set(x => x.Index, value); }
        }

        public MapMapping()
            : this(new AttributeStore())
        {}

        public MapMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<MapMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessMap(this);

            if (Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }

        public override string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }

        public string Sort
        {
            get { return attributes.Get(x => x.Sort); }
            set { attributes.Set(x => x.Sort, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<MapMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<MapMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<MapMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}