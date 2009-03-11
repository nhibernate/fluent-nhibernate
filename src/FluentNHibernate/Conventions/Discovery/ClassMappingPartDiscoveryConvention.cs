using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IMappingPartConvention"/> implementations and applies them to
    /// all <see cref="IMappingPart"/>s in an <see cref="IClassMap"/> instance.
    /// </summary>
    public class ClassMappingPartDiscoveryConvention : BaseMappingPartDiscoveryConvention<IClassMap>, IClassConvention
    {
        public ClassMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}