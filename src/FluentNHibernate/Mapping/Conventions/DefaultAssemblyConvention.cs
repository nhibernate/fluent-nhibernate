using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Conventions
{
    public class DefaultAssemblyConvention : IAssemblyConvention
    {
        private readonly IConventionFinder conventionFinder;

        public DefaultAssemblyConvention(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
        }

        public bool Accept(IEnumerable<IClassMap> target)
        {
            return true;
        }

        public void Apply(IEnumerable<IClassMap> classes, ConventionOverrides overrides)
        {
            var conventions = conventionFinder.Find<IClassConvention>();

            foreach (var classMap in classes)
            {
                foreach (var classConvention in conventions)
                {
                    if (classConvention.Accept(classMap))
                        classConvention.Apply(classMap, overrides);
                }
            }
        }
    }
}