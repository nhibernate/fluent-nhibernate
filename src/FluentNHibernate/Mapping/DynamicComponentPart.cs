using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ClasslikeMapBase<T>, IDynamicComponent, IAccessStrategy<DynamicComponentPart<T>>
    {
        private readonly PropertyInfo propertyInfo;
        private readonly AccessStrategyBuilder<DynamicComponentPart<T>> access;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private readonly DynamicComponentMapping mapping;

        public DynamicComponentPart(PropertyInfo property)
            : this(new DynamicComponentMapping(), property)
        {
        }

        public DynamicComponentPart(DynamicComponentMapping mapping, PropertyInfo property)
        {
            this.mapping = mapping;
            access = new AccessStrategyBuilder<DynamicComponentPart<T>>(this);
            propertyInfo = property;
        }

        public DynamicComponentMapping GetDynamicComponentMapping()
        {
            mapping.Name = propertyInfo.Name;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var dynamicComponent in dynamicComponents)
                mapping.AddDynamicComponent(dynamicComponent.GetDynamicComponentMapping());

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this component mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }
        
        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<DynamicComponentPart<T>> Access
        {
            get { return access; }
        }

        public DynamicComponentPart<T> WithParentReference(Expression<Func<T, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        private DynamicComponentPart<T> WithParentReference(PropertyInfo property)
        {
            mapping.Parent = new ParentMapping
            {
                Name = property.Name
            };

            return this;
        }

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            var propertyMapping = new PropertyMapping
            {
                Name = property.Name,
                PropertyInfo = property
            };

            var propertyMap = new PropertyMap(propertyMapping);

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

            properties.Add(propertyMap); // new

            return propertyMap;
        }

        public override DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property);

            action(part);

            dynamicComponents.Add(part);

            return part;
        }

        #region Explicit IDynamicComponent implementation

        IDynamicComponent IDynamicComponent.WithParentReference<TExplicit>(Expression<Func<TExplicit, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        public int LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        public PartPosition PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        #endregion
    }
}