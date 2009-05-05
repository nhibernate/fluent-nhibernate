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
            var manyToMany = (IManyToManyPart)relationship;

            foreach (var convention in conventions)
            {
                if (convention.Accept(manyToMany))
                    convention.Apply(manyToMany);
            }
        }
    }
}