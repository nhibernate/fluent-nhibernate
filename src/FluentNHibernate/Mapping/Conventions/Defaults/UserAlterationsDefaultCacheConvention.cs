namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsDefaultCacheConvention : IClassConvention
    {
        public bool Accept(IClassMap target)
        {
            return !target.Cache.IsDirty;
        }

        public void Apply(IClassMap target, ConventionOverrides overrides)
        {
            if (overrides.DefaultCache == null) return;

            overrides.DefaultCache(target.Cache);
        }
    }
}