using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class DefaultCacheConvention : IClassConvention
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