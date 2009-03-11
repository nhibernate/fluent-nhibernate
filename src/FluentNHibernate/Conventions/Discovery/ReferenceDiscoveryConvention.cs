using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public class ReferenceDiscoveryConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public ReferenceDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IManyToOnePart);
        }

        public void Apply(IRelationship relationship, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IReferenceConvention>();
            var m2o = (IManyToOnePart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(m2o))
                    convention.Apply(m2o, overrides);
            }
        }
    }
}