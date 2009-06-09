using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
        private readonly CompositeElementMapping mapping = new CompositeElementMapping();
        private readonly IList<PropertyMap> properties = new List<PropertyMap>();
        private readonly IList<IManyToOnePart> references = new List<IManyToOnePart>();

        public PropertyMap Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public PropertyMap Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual PropertyMap Map(PropertyInfo property, string columnName)
        {
            var propertyMap = new PropertyMap(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

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
                part.ColumnName(columnName);

            references.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public CompositeElementPart<T> WithParentReference(Expression<Func<T, object>> exp)
        {
            var property = ReflectionHelper.GetProperty(exp);
            mapping.Parent = new ParentMapping { Name = property.Name };
            return this;
        }

        CompositeElementMapping ICompositeElementMappingProvider.GetCompositeElementMapping()
        {
            if (!mapping.Attributes.IsSpecified(x => x.Class))
                mapping.Class = new TypeReference(typeof(T));

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            return mapping;
        }
    }
}