namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultComponentConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultComponentConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IComponent);
        }

        public void Apply(IMappingPart target, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IComponentConvention>();
            var component = (IComponent)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(component))
                    convention.Apply(component, overrides);
            }
        }
    }
}