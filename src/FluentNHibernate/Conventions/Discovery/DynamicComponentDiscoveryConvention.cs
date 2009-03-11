using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IDynamicComponentConvention"/> implementations and applies them to
    /// an <see cref="IDynamicComponent"/> instance.
    /// </summary>
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