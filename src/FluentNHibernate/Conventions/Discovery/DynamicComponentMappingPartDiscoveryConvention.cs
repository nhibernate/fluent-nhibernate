using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="IDynamicComponent"/> instance.
    /// </summary>
    public class DynamicComponentMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IDynamicComponent>, IDynamicComponentConvention
    {
        public DynamicComponentMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}