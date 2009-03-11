using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class HasManyDiscoveryConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public HasManyDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IOneToManyPart);
        }

        public void Apply(IRelationship relationship, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IHasManyConvention>();
            var o2m = (IOneToManyPart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(o2m))
                    convention.Apply(o2m, overrides);
            }
        }
    }
}