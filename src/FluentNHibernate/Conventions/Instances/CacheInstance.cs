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
            mapping.Usage = "read-write";
        }

        public void NonStrictReadWrite()
        {
            mapping.Usage = "nonstrict-read-write";
        }

        public void ReadOnly()
        {
            mapping.Usage = "read-only";
        }

        public void Custom(string custom)
        {
            mapping.Usage = custom;
        }

        public void Region(string name)
        {
            mapping.Region = name;
        }
    }
}