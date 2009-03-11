namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsReferencesConvention : IReferencesConvention
    {
        public bool Accept(IManyToOnePart target)
        {
            return true;
        }

        public void Apply(IManyToOnePart target, ConventionOverrides overrides)
        {
            if (overrides.ManyToOneConvention != null)
                overrides.ManyToOneConvention(target);
        }
    }
}