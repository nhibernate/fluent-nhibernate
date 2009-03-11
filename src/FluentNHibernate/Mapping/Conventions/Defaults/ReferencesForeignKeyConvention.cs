namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class ReferencesForeignKeyConvention : IReferencesConvention
    {
        public bool Accept(IManyToOnePart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IManyToOnePart target, ConventionOverrides overrides)
        {
            if (overrides.GetForeignKeyName == null)
                target.TheColumnNameIs(target.Property.Name + "_id");
            else
                target.TheColumnNameIs(overrides.GetForeignKeyName(target.Property));
        }
    }
}