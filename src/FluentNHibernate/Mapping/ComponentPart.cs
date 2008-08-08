using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using ShadeTree.Core;
using ShadeTree.Validation;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ClassMapBase<T>, IMappingPart, IAccessStrategy<ComponentPart<T>>
    {
        private readonly PropertyInfo _property;
        private readonly AccessStrategyBuilder<ComponentPart<T>> access;
        private readonly Cache<string, string> properties = new Cache<string, string>();

        public ComponentPart(PropertyInfo property, bool parentIsRequired)
        {
            access = new AccessStrategyBuilder<ComponentPart<T>>(this);
            _property = property;
            this.parentIsRequired = parentIsRequired && RequiredAttribute.IsRequired(_property) && parentIsRequired;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("component")
                .WithAtt("name", _property.Name)
                .WithAtt("insert", "true")
                .WithAtt("update", "true")
                .WithProperties(properties);

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

        public int Level
        {
            get { return 3; }
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<ComponentPart<T>> Access
        {
            get { return access; }
        }
    }
}
