using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="IJoin"/> instance.
    /// </summary>
    public class JoinMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IJoin>, IJoinConvention
    {
        public JoinMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}