using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IPropertyConvention"/> implementations and applies them to
    /// an <see cref="IProperty"/> instance.
    /// </summary>
    public class PropertyDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public PropertyDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IProperty);
        }

        public void Apply(IMappingPart part)
        {
            var conventions = conventionFinder.Find<IPropertyConvention>();
            var property = (IProperty)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(property))
                    convention.Apply(property);
            }
        }
    }
}