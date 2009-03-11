using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultPropertyConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultPropertyConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IProperty);
        }

        public void Apply(IMappingPart part, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IPropertyConvention>();
            var property = (IProperty)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(property))
                    convention.Apply(property, overrides);
            }
        }
    }
}