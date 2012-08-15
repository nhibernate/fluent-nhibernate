using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ComponentPartBase<T, ComponentPart<T>>, IComponentMappingProvider
    {
        private readonly Type entity;
        private readonly AttributeStore attributes;
        private string columnPrefix;

        public ComponentPart(Type entity, Member property)
            : this(entity, property, new AttributeStore())
        {}

        private ComponentPart(Type entity, Member property, AttributeStore attributes)
            : base(attributes, property)
        {
            this.attributes = attributes;
            this.entity = entity;
        }

        /// <summary>
        /// Sets the prefix for every column defined within the component. To refer to the name of a member that exposes
        /// the component use {property}
        /// </summary>
        /// <param name="prefix"></param>
        public void ColumnPrefix(string prefix)
        {
            columnPrefix = prefix;
        }

        /// <summary>
        /// Specify the lazy-load behaviour
        /// </summary>
        public ComponentPart<T> LazyLoad()
        {
            attributes.Set("Lazy", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        IComponentMapping IComponentMappingProvider.GetComponentMapping()
        {
            return CreateComponentMapping();
        }

        protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            var componentMappingRoot = new ComponentMapping(ComponentType.Component, store, member)
            {
                ContainingEntityType = entity
            };
            componentMappingRoot.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(T)));
            componentMappingRoot.ColumnPrefix = columnPrefix;
            return componentMappingRoot;
        }
    }
}
