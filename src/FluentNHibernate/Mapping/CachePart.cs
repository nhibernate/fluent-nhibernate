using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class CachePart : IMappingPart
    {
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private string usage;

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
            get { return -1; }
        }

        public PartPosition Position
        {
            get { return PartPosition.First; }
        }

        public CachePart AsReadWrite()
        {
            usage = "read-write";
            return this;
        }

        public CachePart AsNonStrictReadWrite()
        {
            usage = "nonstrict-read-write";
            return this;
        }

        public CachePart AsReadOnly()
        {
            usage = "read-only";
            return this;
        }

        public CachePart AsCustom(string custom)
        {
            usage = custom;
            return this;
        }
    }
}