namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultVersionConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultVersionConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart part)
        {
            return (part is IVersion);
        }

        public void Apply(IMappingPart part, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IVersionConvention>();
            var version = (IVersion)part;

            foreach (var convention in conventions)
            {
                if (convention.Accept(version))
                    convention.Apply(version, overrides);
            }
        }
    }
}