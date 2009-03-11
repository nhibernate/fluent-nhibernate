using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class HasOneConvention : IHasOneConvention
    {
        public bool Accept(IOneToOnePart target)
        {
            return true;
        }

        public void Apply(IOneToOnePart target, ConventionOverrides overrides)
        {
            if (overrides.OneToOneConvention != null)
                overrides.OneToOneConvention(target);
        }
    }
}