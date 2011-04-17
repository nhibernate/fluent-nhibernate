using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class CacheInstance : CacheInspector, ICacheInstance
    {
        const int layer = Layer.Conventions;
        readonly CacheMapping mapping;

        public CacheInstance(CacheMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void ReadWrite()
        {
            mapping.Set(x => x.Usage, layer, "read-write");
        }

        public void NonStrictReadWrite()
        {
            mapping.Set(x => x.Usage, layer, "nonstrict-read-write");
        }

        public void ReadOnly()
        {
            mapping.Set(x => x.Usage, layer, "read-only");
        }

        public void Transactional()
        {
            mapping.Set(x => x.Usage, layer, "transactional");
        }

        public void IncludeAll()
        {
            mapping.Set(x => x.Include, layer, "all");
        }

        public void IncludeNonLazy()
        {
            mapping.Set(x => x.Include, layer, "non-lazy");
        }

        public void CustomInclude(string include)
        {
            mapping.Set(x => x.Include, layer, include);
        }

        public void CustomUsage(string custom)
        {
            mapping.Set(x => x.Usage, layer, custom);
        }

        public new void Region(string name)
        {
            mapping.Set(x => x.Region, layer, name);
        }
    }
}