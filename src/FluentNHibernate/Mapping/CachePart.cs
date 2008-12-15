using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class CachePart : IMappingPart
    {
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly string usage;

        public CachePart(string usage)
        {
            this.usage = usage;
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
            var cacheElement = classElement.AddElement("cache");

            cacheElement.SetAttribute("usage", usage);
        }

        public int Level
        {
            get { return 0; }
        }

        public PartPosition Position
        {
            get { return PartPosition.First; }
        }
    }
}