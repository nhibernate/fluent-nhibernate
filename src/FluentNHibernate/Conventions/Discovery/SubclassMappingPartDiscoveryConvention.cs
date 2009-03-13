using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="ISubclass"/> instance.
    /// </summary>
    public class SubclassMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<ISubclass>, ISubclassConvention
    {
        public SubclassMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        { }
    }
}