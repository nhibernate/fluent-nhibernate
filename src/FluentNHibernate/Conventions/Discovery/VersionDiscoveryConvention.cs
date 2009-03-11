using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IVersionConvention"/> implementations and applies them to
    /// an <see cref="IVersion"/> instance.
    /// </summary>
    public class VersionDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public VersionDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IVersion);
        }

        public void Apply(IMappingPart part)
        {
            var conventions = conventionFinder.Find<IVersionConvention>();
            var version = (IVersion)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(version))
                    convention.Apply(version);
            }
        }
    }
}