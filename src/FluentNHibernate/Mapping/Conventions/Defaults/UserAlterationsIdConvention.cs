namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsIdConvention : IIdConvention
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