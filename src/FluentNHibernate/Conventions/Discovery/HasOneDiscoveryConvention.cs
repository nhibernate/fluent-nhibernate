using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class HasOneDiscoveryConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public HasOneDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IOneToOnePart);
        }

        public void Apply(IRelationship relationship, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IHasOneConvention>();
            var o2o = (IOneToOnePart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(o2o))
                    convention.Apply(o2o, overrides);
            }
        }
    }
}