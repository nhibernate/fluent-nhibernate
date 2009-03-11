namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultDynamicComponentConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultDynamicComponentConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IDynamicComponent);
        }

        public void Apply(IMappingPart target, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IDynamicComponentConvention>();
            var component = (IDynamicComponent)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(component))
                    convention.Apply(component, overrides);
            }
        }
    }
}