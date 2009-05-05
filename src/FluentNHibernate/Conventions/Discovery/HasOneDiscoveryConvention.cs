using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IHasOneConvention"/> implementations and applies them to
    /// an <see cref="IOneToOnePart"/> instance.
    /// </summary>
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

        public void Apply(IRelationship relationship)
        {
            var conventions = conventionFinder.Find<IHasOneConvention>();
            var oneToOne = (IOneToOnePart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(oneToOne))
                    convention.Apply(oneToOne);
            }
        }
    }
}