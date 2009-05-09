namespace FluentNHibernate.MappingModel.ClassBased
{
    public class DynamicComponentMapping : ComponentMappingBase
    {

        public DynamicComponentMapping()
            : this(new AttributeStore())
        { }

        private DynamicComponentMapping(AttributeStore store)
            : base(store)
        {
        }
    }
}