namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class UserAlterationsDynamicUpdateConvention : IClassConvention
    {
        public bool Accept(IClassMap target)
        {
            return !target.Attributes.Has("dynamic-update");
        }

        public void Apply(IClassMap target, ConventionOverrides overrides)
        {
            if (overrides.DynamicUpdate == null) return;

            var value = overrides.DynamicUpdate(target);

            if (value == true)
                target.DynamicUpdate();
            else if (value == false)
                target.Not.DynamicUpdate();
        }
    }
}