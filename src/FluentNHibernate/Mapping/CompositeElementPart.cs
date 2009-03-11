using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Component-element for component HasMany's.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class CompositeElementPart<T> : ClasslikeMapBase<T>, IMappingPart
    {
        private readonly Cache<string, string> properties = new Cache<string, string>();

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("composite-element")
                .WithAtt("class", typeof(T).AssemblyQualifiedName)
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
    }
}