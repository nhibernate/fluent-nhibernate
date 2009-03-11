namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultMappingPartConvention : IClassConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultMappingPartConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IClassMap target)
        {
            return true;
        }

        public void Apply(IClassMap target, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IMappingPartConvention>();
            
            foreach (var part in target.Parts)
            {
                foreach (var convention in conventions)
                {
                    if (convention.Accept(part))
                        convention.Apply(part, overrides);
                }
            }
        }
    }
}