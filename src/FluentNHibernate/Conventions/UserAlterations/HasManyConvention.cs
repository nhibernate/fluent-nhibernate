using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class HasManyConvention : IHasManyConvention
    {
        public bool Accept(IOneToManyPart target)
        {
            return true;
        }

        public void Apply(IOneToManyPart target, ConventionOverrides overrides)
        {
            if (overrides.OneToManyConvention != null)
                overrides.OneToManyConvention(target);
        }
    }
}