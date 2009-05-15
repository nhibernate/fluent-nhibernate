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
            var propertyMapping = new PropertyMapping
            {
                Name = property.Name,
                PropertyInfo = property
            };

            var propertyMap = new PropertyMap(propertyMapping);

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


        private readonly Cache<string, string> localProperties = new Cache<string, string>();
        private PropertyInfo parentReference;

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("composite-element")
                .WithAtt("class", typeof(T).AssemblyQualifiedName)
                .WithProperties(localProperties);

            if (parentReference != null)
                element.AddElement("parent").WithAtt("name", parentReference.Name);

            //Write the parts
            m_Parts.Sort(new MappingPartComparer(m_Parts));
            foreach (IMappingPart part in m_Parts)
            {
                part.Write(element, visitor);
            }
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
            parentReference = property;
            return this;
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this component mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        /// <include file='' path='[@name=""]'/>
        public void SetAttribute(string name, string value)
        {
            localProperties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int LevelWithinPosition
        {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }
    }
}