using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultReferencesConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultReferencesConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IRelationship relationship)
        {
            return (relationship is IManyToOnePart);
        }

        public void Apply(IRelationship relationship, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IReferencesConvention>();
            var m2o = (IManyToOnePart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(m2o))
                    convention.Apply(m2o, overrides);
            }
        }
    }
}