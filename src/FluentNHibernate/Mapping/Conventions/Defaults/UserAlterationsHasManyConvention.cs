namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsHasManyConvention : IHasManyConvention
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