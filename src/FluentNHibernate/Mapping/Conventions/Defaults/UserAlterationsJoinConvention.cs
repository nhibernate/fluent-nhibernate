namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsJoinConvention : IJoinConvention
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