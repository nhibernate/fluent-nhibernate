namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultClassMappingPartConvention : BaseDefaultMappingPartConvention<IClassMap>, IClassConvention
    {
        public DefaultClassMappingPartConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}