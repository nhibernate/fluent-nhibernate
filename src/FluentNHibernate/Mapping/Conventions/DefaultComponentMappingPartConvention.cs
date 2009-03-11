namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultComponentMappingPartConvention : BaseDefaultMappingPartConvention<IComponent>, IComponentConvention
    {
        public DefaultComponentMappingPartConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        { }
    }
}