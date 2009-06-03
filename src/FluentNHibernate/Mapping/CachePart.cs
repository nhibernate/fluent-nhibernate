using System;
using System.Xml;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface ICache : IMappingPart
    {
        ICache AsReadWrite();
        ICache AsNonStrictReadWrite();
        ICache AsReadOnly();
        ICache AsCustom(string custom);
        ICache Region(string name);
        bool IsDirty { get; }
        CacheMapping GetCacheMapping();
    }

    public class CachePart : ICache
    {
        private readonly CacheMapping mapping = new CacheMapping();

        public CacheMapping GetCacheMapping()
        {
            return mapping;
        }

        public ICache AsReadWrite()
        {
           mapping.Usage = "read-write";
           return this;
        }

        public ICache AsNonStrictReadWrite()
        {
            mapping.Usage = "nonstrict-read-write";
            return this;
        }

        public ICache AsReadOnly()
        {
            mapping.Usage = "read-only";
            return this;
        }

        public ICache AsCustom(string custom)
        {
            mapping.Usage = custom;
            return this;
        }

        public ICache Region(string name)
        {
            mapping.Region = name;
            return this;
        }

        public bool IsDirty
        {
            get { return mapping.Attributes.IsSpecified(x => x.Region) || mapping.Attributes.IsSpecified(x => x.Usage); }
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }
    }
}