using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class JoinMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IJoin>, IJoinConvention
    {
        public JoinMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}