namespace FluentNHibernate.MappingModel.Collections
{
    public class MapMapping : CollectionMappingBase, IIndexedCollectionMapping
    {
        private readonly AttributeStore<SetMapping> attributes;
        public IIndexMapping Index { get; set; }

        public MapMapping()
            : this(new AttributeStore())
        {}

        protected MapMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<SetMapping>(underlyingStore);
        }

        public AttributeStore<SetMapping> Attributes
        {
            get { return attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessMap(this);

            if (Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }

        public string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }

        public string Sort
        {
            get { return attributes.Get(x => x.Sort); }
            set { attributes.Set(x => x.Sort, value); }
        }
    }
}