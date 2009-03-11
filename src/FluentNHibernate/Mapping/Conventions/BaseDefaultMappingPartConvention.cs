namespace FluentNHibernate.Mapping.Conventions
{
    public abstract class BaseDefaultMappingPartConvention<TPart>
        where TPart : IClassMapBase
    {
        private readonly IConventionFinder conventionFinder;

        protected BaseDefaultMappingPartConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(TPart target)
        {
            return true;
        }

        public void Apply(TPart target, ConventionOverrides overrides)
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