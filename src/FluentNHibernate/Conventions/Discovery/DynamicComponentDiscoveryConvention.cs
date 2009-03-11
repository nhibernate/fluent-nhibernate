using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class DynamicComponentDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DynamicComponentDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IDynamicComponent);
        }

        public void Apply(IMappingPart target)
        {
            var conventions = conventionFinder.Find<IDynamicComponentConvention>();
            var component = (IDynamicComponent)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(component))
                    convention.Apply(component);
            }
        }
    }
}