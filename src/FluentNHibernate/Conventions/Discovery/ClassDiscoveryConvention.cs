using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Discovers any <see cref="IClassConvention"/> implementations and applies them to all the
    /// <see cref="IClassMap"/>s in the domain.
    /// </summary>
    public class ClassDiscoveryConvention : IEntireMappingsConvention
    {
        private readonly IConventionFinder conventionFinder;

        public ClassDiscoveryConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IEnumerable<IClassMap> target)
        {
            return true;
        }

        public void Apply(IEnumerable<IClassMap> classes)
        {
            var conventions = conventionFinder.Find<IClassConvention>();

            foreach (var classMap in classes)
            {
                foreach (var classConvention in conventions)
                {
                    if (classConvention.Accept(classMap))
                        classConvention.Apply(classMap);
                }
            }
        }
    }
}