using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class DynamicComponentMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IDynamicComponent>, IDynamicComponentConvention
    {
        public DynamicComponentMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}