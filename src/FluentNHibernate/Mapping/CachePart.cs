using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface ICache : IMappingPart
    {
        void AsReadWrite();
        void AsNonStrictReadWrite();
        void AsReadOnly();
        void AsCustom(string custom);
        bool IsDirty { get; }
    }

    public class CachePart : ICache
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
            if (!IsDirty) return;

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

        public void AsReadWrite()
        {
            usage = "read-write";
        }

        public void AsNonStrictReadWrite()
        {
            usage = "nonstrict-read-write";
        }

        public void AsReadOnly()
        {
            usage = "read-only";
        }

        public void AsCustom(string custom)
        {
            usage = custom;
        }

        public bool IsDirty
        {
            get { return !string.IsNullOrEmpty(usage); }
        }
    }
}