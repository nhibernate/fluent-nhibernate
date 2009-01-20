using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class BagMapping : CollectionMappingBase
    {
        private readonly AttributeStore<BagMapping> _attributes;

        public BagMapping() : this(new AttributeStore())
        {
            
        }

        public BagMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            _attributes = new AttributeStore<BagMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessBag(this);            
            base.AcceptVisitor(visitor);
        }

        public AttributeStore<BagMapping> Attributes
        {
            get { return _attributes; }
        }

        public string OrderBy
        {
            get { return _attributes.Get(x => x.OrderBy); }
            set { _attributes.Set(x => x.OrderBy, value); }
        }
    }

    //public class BagAttributes : CollectionAttributes
    //{
    //    private readonly AttributeStore<BagAttributes> _store;

    //    public BagAttributes()
    //        : this(new AttributeStore())
    //    {

    //    }

    //    protected BagAttributes(AttributeStore underlyingStore) : base(underlyingStore)
    //    {
    //        _store = new AttributeStore<BagAttributes>(underlyingStore);            
    //    }

    //    public bool IsSpecified(Expression<Func<BagAttributes, object>> exp)
    //    {
    //        return _store.IsSpecified(exp);
    //    }

    //    public string OrderBy
    //    {
    //        get { return _store.Get(x => x.OrderBy); }
    //        set { _store.Set(x => x.OrderBy, value); }
    //    }

    //}
}