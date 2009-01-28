using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class BagMapping : CollectionMappingBase
    {
        private readonly AttributeStore<BagMapping> _attributes;

        public BagMapping() : this(new AttributeStore())
        {
            
        }

        protected BagMapping(AttributeStore underlyingStore)
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

}