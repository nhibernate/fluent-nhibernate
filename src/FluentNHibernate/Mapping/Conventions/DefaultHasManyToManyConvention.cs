using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultHasManyToManyConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultHasManyToManyConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IManyToManyPart);
        }

        public void Apply(IRelationship relationship, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IHasManyToManyConvention>();
            var m2m = (IManyToManyPart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(m2m))
                    convention.Apply(m2m, overrides);
            }
        }
    }
}