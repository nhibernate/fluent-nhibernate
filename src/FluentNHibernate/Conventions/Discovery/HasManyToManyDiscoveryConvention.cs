using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IHasManyToManyConvention"/> implementations and applies them to
    /// an <see cref="IManyToManyPart"/> instance.
    /// </summary>
    public class HasManyToManyDiscoveryConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public HasManyToManyDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IManyToManyPart);
        }

        public void Apply(IRelationship relationship)
        {
            var conventions = conventionFinder.Find<IHasManyToManyConvention>();
            var m2m = (IManyToManyPart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(m2m))
                    convention.Apply(m2m);
            }
        }
    }
}