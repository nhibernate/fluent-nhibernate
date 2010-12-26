using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Visitors
{
    public class ComponentResolutionContext
    {
        public Type ComponentType { get; set; }
        public Member ComponentMember { get; set; }
        public Type EntityType { get; set; }
    }

    public interface IComponentReferenceResolver
    {
        ExternalComponentMapping Resolve(ComponentResolutionContext context, IEnumerable<IExternalComponentMappingProvider> componentProviders);
    }

    public class ComponentMapComponentReferenceResolver : IComponentReferenceResolver
    {
        public ExternalComponentMapping Resolve(ComponentResolutionContext context, IEnumerable<IExternalComponentMappingProvider> componentProviders)
        {
            var providers = componentProviders.Where(x => x.Type == context.ComponentType);

            if (providers.Count() > 1)
                throw new AmbiguousComponentReferenceException(context.ComponentType, context.EntityType, context.ComponentMember);

            var provider = providers.SingleOrDefault();

            if (provider == null)
                return null;

            return provider.GetComponentMapping();
        }
    }

    public class ComponentReferenceResolutionVisitor : DefaultMappingModelVisitor
    {
        readonly IEnumerable<IComponentReferenceResolver> resolvers;
        readonly IEnumerable<IExternalComponentMappingProvider> componentProviders;

        public ComponentReferenceResolutionVisitor(IEnumerable<IComponentReferenceResolver> resolvers, IEnumerable<IExternalComponentMappingProvider> componentProviders)
        {
            this.resolvers = resolvers;
            this.componentProviders = componentProviders;
        }

        public override void ProcessComponent(ReferenceComponentMapping mapping)
        {
            var context = new ComponentResolutionContext
            {
                ComponentType = mapping.Type,
                ComponentMember = mapping.Member,
                EntityType = mapping.ContainingEntityType
            };
            var component = resolvers
                .Select(x => x.Resolve(context, componentProviders))
                .FirstOrDefault(x => x != null);

            if (component == null)
                throw new MissingExternalComponentException(mapping.Type, mapping.ContainingEntityType, mapping.Member);

            mapping.AssociateExternalMapping(component);
            mapping.MergedModel.AcceptVisitor(this);
        }
    }
}
