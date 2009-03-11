using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="IComponent"/> instance.
    /// </summary>
    public class ComponentMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IComponent>, IComponentConvention
    {
        public ComponentMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        { }
    }
}