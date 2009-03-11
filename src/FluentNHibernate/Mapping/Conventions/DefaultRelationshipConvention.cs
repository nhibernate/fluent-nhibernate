using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultRelationshipConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultRelationshipConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IRelationship);
        }

        public void Apply(IMappingPart part, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IRelationshipConvention>();
            var relationship = (IRelationship)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(relationship))
                    convention.Apply(relationship, overrides);
            }
        }
    }
}