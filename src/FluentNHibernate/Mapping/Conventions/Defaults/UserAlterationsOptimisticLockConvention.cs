namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsOptimisticLockConvention : IClassConvention
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