using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class ClassMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IClassMap>, IClassConvention
    {
        public ClassMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}