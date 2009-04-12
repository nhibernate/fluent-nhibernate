using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface ICache : IMappingPart
    {
        ICache AsReadWrite();
        ICache AsNonStrictReadWrite();
        ICache AsReadOnly();
        ICache AsCustom(string custom);
        ICache Region(string name);
        new ICache SetAttribute(string name, string value);
        new ICache SetAttributes(Attributes attrs);
        bool IsDirty { get; }
    }

    public class CachePart : ICache
    {
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private string usage;
        private string region;

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            if (!IsDirty) return;

            var cacheElement = classElement.AddElement("cache")
                  .WithAtt("usage", usage);
            if (!string.IsNullOrEmpty(region))
                cacheElement.WithAtt("region", region);
            cacheElement.WithProperties(attributes);
        }

        public int LevelWithinPosition
        {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.First; }
        }

        public ICache AsReadWrite()
        {
            usage = "read-write";
           return this;
        }
 
        public ICache AsNonStrictReadWrite()
        {
            usage = "nonstrict-read-write";
            return this;
        }
 
        public ICache AsReadOnly()
        {
            usage = "read-only";
            return this;
        }
 
        public ICache AsCustom(string custom)
        {
            usage = custom;
            return this;
        }
 
        public ICache Region(string name)
        {
            region = name;
            return this;
        }

        public ICache SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
            return this;
        }

        public ICache SetAttributes(Attributes attrs)
        {
            foreach (var key in attrs.Keys)
            {
                SetAttribute(key, attrs[key]);
            }
            return this;
        }

        void IHasAttributes.SetAttributes(Attributes attrs)
        {
            SetAttributes(attrs);
        }

        void IHasAttributes.SetAttribute(string name, string value)
        {
            SetAttribute(name, value);
        }

        public bool IsDirty
        {
            get { return !string.IsNullOrEmpty(usage); }
        }
    }
}