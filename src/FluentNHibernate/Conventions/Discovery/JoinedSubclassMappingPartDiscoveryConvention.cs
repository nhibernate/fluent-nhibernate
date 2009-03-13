using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="IJoinedSubclass"/> instance.
    /// </summary>
    public class JoinedSubclassMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IJoinedSubclass>, IJoinedSubclassConvention
    {
        public JoinedSubclassMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        { }
    }
}