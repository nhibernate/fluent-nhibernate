using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IDynamicComponent : IClasslike, IMappingPart
    {
        IDynamicComponent WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp);
    }

    public class DynamicComponentPart<T> : ClasslikeMapBase<T>, IDynamicComponent, IAccessStrategy<DynamicComponentPart<T>>
    {
        private readonly PropertyInfo _property;
        private readonly AccessStrategyBuilder<DynamicComponentPart<T>> access;
        private readonly Cache<string, string> properties = new Cache<string, string>();
        private PropertyInfo _parentReference;

        public DynamicComponentPart(PropertyInfo property, bool parentIsRequired)
        {
            access = new AccessStrategyBuilder<DynamicComponentPart<T>>(this);
            _property = property;
            //TODO: Need some support for this
            //this.parentIsRequired = parentIsRequired && RequiredAttribute.IsRequired(_property) && parentIsRequired;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("dynamic-component")
                .WithAtt("name", _property.Name)
                .WithAtt("insert", "true")
                .WithAtt("update", "true")
                .WithProperties(properties);

            if (_parentReference != null)
                element.AddElement("parent").WithAtt("name", _parentReference.Name);

            writeTheParts(element, visitor);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this component mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int Level
        {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
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
            _parentReference = property;
            return this;
        }

        #region Explicit IDynamicComponent implementation

        IDynamicComponent IDynamicComponent.WithParentReference<TExplicit>(Expression<Func<TExplicit, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        #endregion
    }
}