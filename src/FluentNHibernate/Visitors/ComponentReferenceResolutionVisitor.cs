using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Visitors
{
    public class ComponentReferenceResolutionVisitor : DefaultMappingModelVisitor
    {
        private readonly IEnumerable<IExternalComponentMappingProvider> componentProviders;

        public ComponentReferenceResolutionVisitor(IEnumerable<IExternalComponentMappingProvider> componentProviders)
        {
            this.componentProviders = componentProviders;
        }

        public override void ProcessComponent(ReferenceComponentMapping mapping)
        {
            var providers = componentProviders.Where(x => x.Type == mapping.Type);

            if (!providers.Any())
                throw new MissingExternalComponentException(mapping.Type, mapping.ContainingEntityType, mapping.Member);
            if (providers.Count() > 1)
                throw new AmbiguousComponentReferenceException(mapping.Type, mapping.ContainingEntityType, mapping.Member);

            mapping.AssociateExternalMapping(providers.Single().GetComponentMapping());
        }
    }
}
