namespace FluentNHibernate.MappingModel.Collections
{
    public class BagMapping : CollectionMappingBase
    {
        private readonly AttributeStore<BagMapping> attributes;

        public BagMapping() : this(new AttributeStore())
        {
            
        }

        protected BagMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<BagMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessBag(this);
            base.AcceptVisitor(visitor);
        }

        public AttributeStore<BagMapping> Attributes
        {
            get { return attributes; }
        }

        public string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }
    }

}