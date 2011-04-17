using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Component-element for component HasMany's.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class CompositeElementPart<T> : ICompositeElementMappingProvider, INestedCompositeElementMappingProvider
    {
        readonly Type entity;
        readonly Member member;
        readonly List<IPropertyMappingProvider> properties = new List<IPropertyMappingProvider>();
        readonly List<IManyToOneMappingProvider> references = new List<IManyToOneMappingProvider>();
        readonly List<INestedCompositeElementMappingProvider> components = new List<INestedCompositeElementMappingProvider>();
        readonly AttributeStore attributes = new AttributeStore();

        public CompositeElementPart(Type entity)
        {
            this.entity = entity;
        }

        public CompositeElementPart(Type entity, Member member)
            : this(entity)
        {
            this.member = member;
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="expression">Property</param>
        /// <example>
        /// Map(x => x.Age);
        /// </example>
        public PropertyPart Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="expression">Property</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// Map(x => x.Age, "person_age");
        /// </example>
        public PropertyPart Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(expression.ToMember(), columnName);
        }

        protected virtual PropertyPart Map(Member property, string columnName)
        {
            var propertyMap = new PropertyPart(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.Column(columnName);

            properties.Add(propertyMap);

            return propertyMap;
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="expression">Property on the current entity</param>
        /// <example>
        /// References(x => x.Company);
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression)
        {
            return References(expression, null);
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="expression">Property on the current entity</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// References(x => x.Company, "person_company_id");
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression, string columnName)
        {
            return References<TOther>(expression.ToMember(), columnName);
        }

        protected virtual ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            var part = new ManyToOnePart<TOther>(typeof(T), property);

            if (columnName != null)
                part.Column(columnName);

            references.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="expression">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public void ParentReference(Expression<Func<T, object>> expression)
        {
            var parentMapping = new ParentMapping
            {
                ContainingEntityType = entity
            };
            parentMapping.Set(x => x.Name, Layer.Defaults, expression.ToMember().Name);
            attributes.Set("Parent", Layer.Defaults, parentMapping);
        }

        /// <summary>
        /// Create a nested component mapping.
        /// </summary>
        /// <param name="property">Component property</param>
        /// <param name="nestedCompositeElementAction">Action for creating the component</param>
        /// <example>
        /// HasMany(x => x.Locations)
        ///   .Component(c =>
        ///   {
        ///     c.Map(x => x.Name);
        ///     c.Component(x => x.Address, addr =>
        ///     {
        ///       addr.Map(x => x.Street);
        ///       addr.Map(x => x.PostCode);
        ///     });
        ///   });
        /// </example>
        public void Component<TChild>(Expression<Func<T, TChild>> property, Action<CompositeElementPart<TChild>> nestedCompositeElementAction)
        {
            var nestedCompositeElement = new CompositeElementPart<TChild>(entity, property.ToMember());

            nestedCompositeElementAction(nestedCompositeElement);

            components.Add(nestedCompositeElement);
        }

        void PopulateMapping(CompositeElementMapping mapping)
        {
            mapping.ContainingEntityType = entity;
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(T)));

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var component in components)
                mapping.AddCompositeElement(component.GetCompositeElementMapping());
        }

        CompositeElementMapping ICompositeElementMappingProvider.GetCompositeElementMapping()
        {
            var mapping = new CompositeElementMapping(attributes.Clone());

            PopulateMapping(mapping);

            return mapping;
        }

        NestedCompositeElementMapping INestedCompositeElementMappingProvider.GetCompositeElementMapping()
        {
            var mapping = new NestedCompositeElementMapping(attributes.Clone());
            mapping.Set(x => x.Name, Layer.Defaults, member.Name);

            PopulateMapping(mapping);

            return mapping;
        }
    }
}