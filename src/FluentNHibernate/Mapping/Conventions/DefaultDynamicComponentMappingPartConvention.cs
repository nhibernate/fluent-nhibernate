namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultDynamicComponentMappingPartConvention : BaseDefaultMappingPartConvention<IDynamicComponent>, IDynamicComponentConvention
    {
        public DefaultDynamicComponentMappingPartConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}