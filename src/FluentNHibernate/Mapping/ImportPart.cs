using System;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ImportPart : IMappingPart
    {
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly Type importType;

        public ImportPart(Type importType)
        {
            this.importType = importType;
        }

        public void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public void SetAttributes(Attributes attrs)
        {
            foreach (var key in attrs.Keys)
            {
                SetAttribute(key, attrs[key]);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var importElement = classElement.AddElement("import")
                .WithAtt("class", importType.AssemblyQualifiedName)
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            attributes.ForEachPair((name, value) => importElement.WithAtt(name, value));
        }

        public void As(string alternativeName)
        {
            SetAttribute("rename", alternativeName);
        }

        public int LevelWithinPosition
        {
            get { return -1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.First; }
        }
    }
}