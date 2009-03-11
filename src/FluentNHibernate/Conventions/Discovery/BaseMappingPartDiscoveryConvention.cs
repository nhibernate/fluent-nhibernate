using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    public abstract class BaseMappingPartDiscoveryConvention<TPart>
        where TPart : IClassMapBase
    {
        private readonly IConventionFinder conventionFinder;

        protected BaseMappingPartDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(TPart target)
        {
            return true;
        }

        public void Apply(TPart target)
        {
            var conventions = conventionFinder.Find<IMappingPartConvention>();

            foreach (var part in target.Parts)
            {
                foreach (var convention in conventions)
                {
                    if (convention.Accept(part))
                        convention.Apply(part);
                }
            }
        }
    }
}