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
    public class CompositeElementPart<T> : ICompositeElementMappingProvider
    {
        private readonly Type entity;
        private readonly IList<IPropertyMappingProvider> properties = new List<IPropertyMappingProvider>();
        private readonly IList<IManyToOneMappingProvider> references = new List<IManyToOneMappingProvider>();
        private readonly AttributeStore<CompositeElementMapping> attributes = new AttributeStore<CompositeElementMapping>();

        public CompositeElementPart(Type entity)
        {
            this.entity = entity;
        }

        public PropertyPart Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public PropertyPart Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual PropertyPart Map(PropertyInfo property, string columnName)
        {
            var propertyMap = new PropertyPart(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.Column(columnName);

            properties.Add(propertyMap);

            return propertyMap;
        }

        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression)
        {
            return References(expression, null);
        }

        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression, string columnName)
        {
            return References<TOther>(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual ManyToOnePart<TOther> References<TOther>(PropertyInfo property, string columnName)
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
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public CompositeElementPart<T> ParentReference(Expression<Func<T, object>> exp)
        {
            var property = ReflectionHelper.GetProperty(exp);
            attributes.Set(x => x.Parent, new ParentMapping
            {
                Name = property.Name,
                ContainingEntityType = entity
            });
            return this;
        }

        CompositeElementMapping ICompositeElementMappingProvider.GetCompositeElementMapping()
        {
            var mapping = new CompositeElementMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;

            if (!mapping.IsSpecified(x => x.Class))
                mapping.Class = new TypeReference(typeof(T));

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            return mapping;
        }
    }
}