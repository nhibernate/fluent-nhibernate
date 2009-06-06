namespace FluentNHibernate.MappingModel.Collections
{
    public class ListMapping : CollectionMappingBase
    {
        private readonly AttributeStore<ListMapping> attributes;
        public IndexMapping Index { get; set; }

        public ListMapping()
            : this(new AttributeStore())
        {}

        protected ListMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            attributes = new AttributeStore<ListMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessList(this);

            if(Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }

        public AttributeStore<ListMapping> Attributes
        {
            get { return attributes; }
        }
    }
}
