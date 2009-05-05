namespace FluentNHibernate.MappingModel.Collections
{
    public class SetMapping : CollectionMappingBase
    {
        private readonly AttributeStore<SetMapping> attributes;

        public SetMapping() : this(new AttributeStore())
        {
            
        }
        
        protected SetMapping(AttributeStore underlyingStore) : base(underlyingStore)
        {
            attributes = new AttributeStore<SetMapping>(underlyingStore);
        }

        public AttributeStore<SetMapping> Attributes
        {
            get { return attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessSet(this);
            base.AcceptVisitor(visitor);
        }

        public string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }
    }
}