using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class JoinConvention : IJoinConvention
    {
        public bool Accept(IJoin target)
        {
            return true;
        }

        public void Apply(IJoin target, ConventionOverrides overrides)
        {
            if (overrides.JoinConvention != null)
                overrides.JoinConvention(target);
        }
    }
}