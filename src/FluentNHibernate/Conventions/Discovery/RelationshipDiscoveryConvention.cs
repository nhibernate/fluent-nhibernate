using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IRelationshipConvention"/> implementations and applies them to
    /// an <see cref="IRelationship"/> instance.
    /// </summary>
    public class RelationshipDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public RelationshipDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IRelationship);
        }

        public void Apply(IMappingPart part)
        {
            var conventions = conventionFinder.Find<IRelationshipConvention>();
            var relationship = (IRelationship)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(relationship))
                    convention.Apply(relationship);
            }
        }
    }
}