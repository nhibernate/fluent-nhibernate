using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T, DynamicComponentPart<T>>, IComponentMappingProvider
    {
        private readonly Type entity;
        private readonly MappingProviderStore providers;
        private readonly AccessStrategyBuilder<DynamicComponentPart<T>> access;
        private readonly AttributeStore<ComponentMapping> attributes;

        public DynamicComponentPart(Type entity, Member member)
            : this(entity, member, new AttributeStore(), new MappingProviderStore())
        {}

        private DynamicComponentPart(Type entity, Member member, AttributeStore underlyingStore, MappingProviderStore providers)
            : base(underlyingStore, member, providers)
        {
            this.entity = entity;
            this.providers = providers;
            attributes = new AttributeStore<ComponentMapping>(underlyingStore);
            access = new AccessStrategyBuilder<DynamicComponentPart<T>>(this, value => attributes.Set(x => x.Access, value));
        }

        protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            return new ComponentMapping(ComponentType.DynamicComponent, store)
            {
                ContainingEntityType = entity
            };
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <example>
        /// Map("Age");
        /// </example>
        public PropertyPart Map(string key)
        {
            return Map<string>(key);
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <typeparam name="TProperty">Property type</typeparam>
        /// <example>
        /// Map&lt;int&gt;("Age");
        /// </example>
        public PropertyPart Map<TProperty>(string key)
        {
            var property = new DummyPropertyInfo(key, typeof(TProperty));
            var propertyMap = new PropertyPart(property.ToMember(), typeof(T));

            providers.Properties.Add(propertyMap);

            return propertyMap;
        }

        IComponentMapping IComponentMappingProvider.GetComponentMapping()
        {
            return CreateComponentMapping();
        }
    }
}