using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{

    public class ComponentPart<T> : ClasslikeMapBase<T>, IComponent, IAccessStrategy<ComponentPart<T>>
    {
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<ComponentPart<T>> access;
        private new readonly Cache<string, string> properties = new Cache<string, string>();
        private PropertyInfo parentReference;

        public ComponentPart(PropertyInfo property, bool parentIsRequired)
        {
            access = new AccessStrategyBuilder<ComponentPart<T>>(this);
            this.property = property;
			//TODO: Need some support for this
            //this.parentIsRequired = parentIsRequired && RequiredAttribute.IsRequired(this.property) && parentIsRequired;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("component")
                .WithAtt("name", property.Name)
                .WithAtt("insert", "true")
                .WithAtt("update", "true")
                .WithProperties(properties);

            if (parentReference != null)
                element.AddElement("parent").WithAtt("name", parentReference.Name);

            WriteTheParts(element, visitor);
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
            parentReference = property;
            return this;
        }

        #region Explicit IComponent implementation

        IComponent IComponent.WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        public ComponentMapping GetComponentMapping()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
