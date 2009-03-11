using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultHasOneConvention : IRelationshipConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultHasOneConvention(IConventionFinder conventionFinder)
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