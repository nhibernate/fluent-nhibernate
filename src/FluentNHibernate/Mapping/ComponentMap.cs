using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Defines a mapping for a component. Derive from this class to create a mapping,
    /// and use the constructor to control how your component is persisted.
    /// </summary>
    /// <example>
    /// public class AddressMap : ComponentMap&lt;Address&gt;
    /// {
    ///   public AddressMap()
    ///   {
    ///     Map(x => x.Street);
    ///     Map(x => x.City);
    ///   }
    /// }
    /// </example>
    /// <typeparam name="T">Component type to map</typeparam>
    public class ComponentMap<T> : ComponentPartBase<T, ComponentMap<T>>, IExternalComponentMappingProvider
    {
        private readonly AttributeStore<ComponentMapping> attributes;

        public ComponentMap()
            : this(new AttributeStore())
        {}

        internal ComponentMap(AttributeStore underlyingStore)
            : base(underlyingStore, null)
        {
            attributes = new AttributeStore<ComponentMapping>(underlyingStore);
        }

        /// <summary>
        /// Creates a component reference. This is a place-holder for a component that is defined externally with a
        /// <see cref="ComponentMap{T}"/>; the mapping defined in said <see cref="ComponentMap{T}"/> will be merged
        /// with any options you specify from this call.
        /// </summary>
        /// <typeparam name="TComponent">Component type</typeparam>
        /// <param name="member">Property exposing the component</param>
        /// <returns>Component reference builder</returns>
        public override ReferenceComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> member)
        {
            if (typeof(TComponent) == typeof(T))
                throw new NotSupportedException("Nested components of the same type are not supported in ComponentMap.");

            return base.Component(member);
        }

        protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            return new ExternalComponentMapping(ComponentType.Component, attributes.CloneInner());
        }

        ExternalComponentMapping IExternalComponentMappingProvider.GetComponentMapping()
        {
            var mapping = (ExternalComponentMapping)CreateComponentMapping();

            return mapping.DeepClone();
        }

        Type IExternalComponentMappingProvider.Type
        {
            get { return typeof(T); }
        }
    }
}