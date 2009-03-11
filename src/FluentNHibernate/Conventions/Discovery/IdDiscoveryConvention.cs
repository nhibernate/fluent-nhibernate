using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IIdConvention"/> implementations and applies them to
    /// an <see cref="IIdentityPart"/> instance.
    /// </summary>
    public class IdDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public IdDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IIdentityPart);
        }

        public void Apply(IMappingPart part)
        {
            var conventions = conventionFinder.Find<IIdConvention>();
            var id = (IIdentityPart)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(id))
                    convention.Apply(id);
            }
        }
    }
}