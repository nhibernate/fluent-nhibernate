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

        public void Custom(string custom)
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