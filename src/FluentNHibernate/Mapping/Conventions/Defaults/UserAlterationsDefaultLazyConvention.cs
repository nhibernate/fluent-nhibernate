namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsDefaultLazyConvention : IClassConvention
    {
        public bool Accept(IClassMap target)
        {
            return !target.HibernateMappingAttributes.Has("default-lazy");
        }

        public void Apply(IClassMap target, ConventionOverrides overrides)
        {
            target.SetHibernateMappingAttribute("default-lazy", overrides.DefaultLazyLoad);
        }
    }
}