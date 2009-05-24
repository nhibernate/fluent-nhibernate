namespace FluentNHibernate.MappingModel.Collections
{
    public class ArrayMapping : CollectionMappingBase
    {
        public ArrayMapping()
            : this(new AttributeStore())
        {}

        public ArrayMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}
    }
}