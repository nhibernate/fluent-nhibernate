using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IJoinConvention"/> implementations and applies them to
    /// an <see cref="IJoin"/> instance.
    /// </summary>
    public class JoinDiscoveryConvention : IMappingPartConvention
    {
        private readonly IConventionFinder conventionFinder;

        public JoinDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IMappingPart target)
        {
            return (target is IJoin);
        }

        public void Apply(IMappingPart target)
        {
            var conventions = conventionFinder.Find<IJoinConvention>();
            var join = (IJoin)target;

            foreach (var convention in conventions)
            {
                if (convention.Accept(join))
                    convention.Apply(join);
            }
        }
    }
}