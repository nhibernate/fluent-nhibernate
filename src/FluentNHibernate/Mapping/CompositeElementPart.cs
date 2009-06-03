using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.Type;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Component-element for component HasMany's.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class CompositeElementPart<T> : IMappingPart
    {
        protected readonly List<IMappingPart> m_Parts = new List<IMappingPart>();
        public IEnumerable<IMappingPart> Parts
        {
            get { return m_Parts; }
        }
        protected internal void AddPart(IMappingPart part)
        {
            m_Parts.Add(part);
        }

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

            m_Parts.Add(propertyMap);

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

            AddPart(part);

            return part;
        }

        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public CompositeElementPart<T> WithParentReference(Expression<Func<T, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        private CompositeElementPart<T> WithParentReference(PropertyInfo property)
        {
            return this;
        }
    }
}