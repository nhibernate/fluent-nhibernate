using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class CacheInstance : ICacheInstance
    {
        private readonly CacheMapping mapping;

        public CacheInstance(CacheMapping mapping)
        {
            this.mapping = mapping;
        }

        public string Value
        {
            get { return mapping.Usage; }
        }

        public string RegionValue
        {
            get { return mapping.Region; }
        }

        public void ReadWrite()
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = "read-write";
        }

        public void NonStrictReadWrite()
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = "nonstrict-read-write";
        }

        public void ReadOnly()
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = "read-only";
        }

        public void Custom(string custom)
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = custom;
        }

        public void Region(string name)
        {
            if (!mapping.IsSpecified(x => x.Region))
                mapping.Region = name;
        }
    }
}