using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShadeTree.Core;

namespace FluentNHibernate.Mapping
{
    public class SubClassPart<T> : ClassMapBase<T>, IMappingPart
    {
        private readonly string _discriminatorValue;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public SubClassPart(string discriminatorValue)
        {
            _discriminatorValue = discriminatorValue;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement subclassElement = classElement.AddElement("subclass")
                .WithAtt("discriminator-value", _discriminatorValue)
                .WithAtt("name", typeof(T).Name)
                .WithProperties(attributes);

            writeTheParts(subclassElement, visitor);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this sub-class mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public int Level
        {
            get { return 3; }
        }
    }
}
