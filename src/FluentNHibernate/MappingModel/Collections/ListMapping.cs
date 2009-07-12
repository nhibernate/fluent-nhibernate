namespace FluentNHibernate.MappingModel.Collections
{
    public class ListMapping : CollectionMappingBase, IIndexedCollectionMapping
    {
        public IIndexMapping Index { get; set; }

        public ListMapping()
            : this(new AttributeStore())
        {}

        public ListMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessList(this);

            if(Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }
    }
}
