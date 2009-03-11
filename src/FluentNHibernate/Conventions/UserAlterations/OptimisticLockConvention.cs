using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class OptimisticLockConvention : IClassConvention
    {
        public bool Accept(IClassMap target)
        {
            return !target.Attributes.Has("optimistic-lock");
        }

        public void Apply(IClassMap target, ConventionOverrides overrides)
        {
            if (overrides.OptimisticLock == null) return;

            overrides.OptimisticLock(target, target.OptimisticLock);
        }
    }
}