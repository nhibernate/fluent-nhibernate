using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.UserAlterations
{
    public class IdConvention : IIdConvention
    {
        public bool Accept(IIdentityPart target)
        {
            return true;
        }

        public void Apply(IIdentityPart target, ConventionOverrides overrides)
        {
            if (overrides.IdConvention != null)
                overrides.IdConvention(target);
        }
    }
}