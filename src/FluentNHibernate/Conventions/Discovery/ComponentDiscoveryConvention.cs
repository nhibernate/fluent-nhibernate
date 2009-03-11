using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class ComponentDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public ComponentDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IComponent);
        }

        public void Apply(IMappingPart target)
        {
            var conventions = conventionFinder.Find<IComponentConvention>();
            var component = (IComponent)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(component))
                    convention.Apply(component);
            }
        }
    }
}