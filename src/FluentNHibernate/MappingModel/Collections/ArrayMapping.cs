namespace FluentNHibernate.MappingModel.Collections
{
    public class ArrayMapping : CollectionMappingBase, IIndexedCollectionMapping
    {
        public IIndexMapping Index { get; set; }

        public ArrayMapping()
            : this(new AttributeStore())
        {}

        public ArrayMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessArray(this);

            if (Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }
    }
}