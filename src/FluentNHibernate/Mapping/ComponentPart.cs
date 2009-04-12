using System.Reflection;
using System.Xml;
using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IComponent : IClasslike, IMappingPart
    {
        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        IComponent WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp);
    }

    public class ComponentPart<T> : ClasslikeMapBase<T>, IComponent, IAccessStrategy<ComponentPart<T>>
    {
        private readonly PropertyInfo _property;
        private readonly AccessStrategyBuilder<ComponentPart<T>> access;
        private readonly Cache<string, string> properties = new Cache<string, string>();
        private PropertyInfo _parentReference;

        public ComponentPart(PropertyInfo property, bool parentIsRequired)
        {
            access = new AccessStrategyBuilder<ComponentPart<T>>(this);
            _property = property;
			//TODO: Need some support for this
            //this.parentIsRequired = parentIsRequired && RequiredAttribute.IsRequired(_property) && parentIsRequired;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("component")
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

        public int LevelWithinPosition
        {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<ComponentPart<T>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public ComponentPart<T> WithParentReference(Expression<Func<T, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        private ComponentPart<T> WithParentReference(PropertyInfo property)
        {
            _parentReference = property;
            return this;
        }

        #region Explicit IComponent implementation

        IComponent IComponent.WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        #endregion
    }
}
