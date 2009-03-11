namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsHasOneConvention : IHasOneConvention
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