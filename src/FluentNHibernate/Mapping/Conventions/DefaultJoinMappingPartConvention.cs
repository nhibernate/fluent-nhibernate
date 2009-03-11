namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultJoinMappingPartConvention : BaseDefaultMappingPartConvention<IJoin>, IJoinConvention
    {
        public DefaultJoinMappingPartConvention(IConventionFinder conventionFinder)
            : base(conventionFinder)
        {}
    }
}