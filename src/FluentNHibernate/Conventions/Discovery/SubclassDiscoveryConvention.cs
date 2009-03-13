using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="ISubclassConvention"/> implementations and applies them to
    /// an <see cref="ISubclass"/> instance.
    /// </summary>
    public class SubclassDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public SubclassDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is ISubclass);
        }

        public void Apply(IMappingPart target)
        {
            var conventions = conventionFinder.Find<ISubclassConvention>();
            var join = (ISubclass)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(join))
                    convention.Apply(join);
            }
        }
    }
}