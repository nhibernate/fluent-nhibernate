using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class CacheInstance : CacheInspector, ICacheInstance
    {
        private readonly CacheMapping mapping;

        public CacheInstance(CacheMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
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

        public void Transactional()
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = "transactional";
        }

        public void IncludeAll()
        {
            if (!mapping.IsSpecified(x => x.Include))
                mapping.Include = "all";
        }

        public void IncludeNonLazy()
        {
            if (!mapping.IsSpecified(x => x.Include))
                mapping.Include = "non-lazy";
        }

        public void CustomInclude(string include)
        {
            if (!mapping.IsSpecified(x => x.Include))
                mapping.Include = include;
        }

        public void CustomUsage(string custom)
        {
            if (!mapping.IsSpecified(x => x.Usage))
                mapping.Usage = custom;
        }

        public new void Region(string name)
        {
            if (!mapping.IsSpecified(x => x.Region))
                mapping.Region = name;
        }
    }
}