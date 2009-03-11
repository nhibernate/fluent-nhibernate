namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class HasManyForeignKeyNameConvention : IHasManyConvention
    {
        public bool Accept(IOneToManyPart target)
        {
            return string.IsNullOrEmpty(target.ColumnName);
        }

        public void Apply(IOneToManyPart target, ConventionOverrides overrides)
        {
            if (overrides.GetForeignKeyNameForType == null)
                target.WithKeyColumn(target.ParentType.Name + "_id");
            else
                target.WithKeyColumn(overrides.GetForeignKeyNameForType(target.ParentType));
        }
    }
}