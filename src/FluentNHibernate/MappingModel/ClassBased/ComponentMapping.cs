namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentMapping : ComponentMappingBase
    {
        public ComponentMapping()
            : this(new AttributeStore())
        {}

        private ComponentMapping(AttributeStore store)
            : base(store)
        {
        }
    }
}