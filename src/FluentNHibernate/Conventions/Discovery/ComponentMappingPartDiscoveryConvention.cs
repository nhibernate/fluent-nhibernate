using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class ComponentMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IComponent>, IComponentConvention
    {
        public ComponentMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        { }
    }
}