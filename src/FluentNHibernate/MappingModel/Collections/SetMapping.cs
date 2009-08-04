using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Collections
{
    public class SetMapping : CollectionMappingBase
    {
        private readonly AttributeStore<SetMapping> attributes;

        public SetMapping()
            : this(new AttributeStore())
        {}
        
        public SetMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<SetMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessSet(this);
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

        public bool IsSpecified<TResult>(Expression<Func<SetMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<SetMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<SetMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}