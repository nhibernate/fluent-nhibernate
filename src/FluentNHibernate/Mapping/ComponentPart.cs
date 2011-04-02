using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ComponentPartBase<T, ComponentPart<T>>, IComponentMappingProvider
    {
        private readonly Type entity;
        private readonly AttributeStore<ComponentMapping> attributes;

        public ComponentPart(Type entity, Member property)
            : this(entity, property, new AttributeStore())
        {}

        private ComponentPart(Type entity, Member property, AttributeStore underlyingStore)
            : base(underlyingStore, property)
        {
            attributes = new AttributeStore<ComponentMapping>(underlyingStore);
            this.entity = entity;

            Insert();
            Update();
        }

        /// <summary>
        /// Specify the lazy-load behaviour
        /// </summary>
        public ComponentPart<T> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        IComponentMapping IComponentMappingProvider.GetComponentMapping()
        {
            return CreateComponentMapping();
        }

        protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            return new ComponentMapping(ComponentType.Component, store)
            {
                ContainingEntityType = entity,
                Class = new TypeReference(typeof(T))
            };
        }
    }
}
