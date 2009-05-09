using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Discovery
{
    /// <summary>
    /// Base implementation for "classlike" mappings. Finds and applies any <see cref="IMappingPartConvention"/>
    /// implementations to all <see cref="IMappingPart"/>s in the classlike container.
    /// </summary>
    /// <remarks>
    /// Classlike mappings are ones that are like classes in their capabilities, components
    /// and join's are two examples.
    /// </remarks>
    /// <typeparam name="TPart">Classlike mapping</typeparam>
    public abstract class BaseMappingPartDiscoveryConvention<TPart>
        where TPart : IClasslike
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

            ApplyConventions(conventions, target.Parts);
            ApplyConventions(conventions, target.Properties);
            ApplyConventions(conventions, target.Subclasses);
            ApplyConventions(conventions, target.JoinedSubclasses);
            ApplyConventions(conventions, target.Components);
        }

        private void ApplyConventions(IEnumerable<IMappingPartConvention> conventions, IEnumerable parts)
        {
            if (parts == null) return;

            foreach (IMappingPart part in parts)
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