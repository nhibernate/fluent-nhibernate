namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public bool Accept(IIdentityPart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IIdentityPart target, ConventionOverrides overrides)
        {
            if (overrides.GetPrimaryKeyName == null)
                target.TheColumnNameIs(target.Property.Name);
            else
                target.TheColumnNameIs(overrides.GetPrimaryKeyName(target));
        }
    }
}