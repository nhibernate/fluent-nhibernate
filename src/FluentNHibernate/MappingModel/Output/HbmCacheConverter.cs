using System.Linq;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCacheConverter : HbmConverterBase<CacheMapping, HbmCache>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCacheUsage> usageDict = new XmlLinkedEnumBiDictionary<HbmCacheUsage>();
        private static readonly XmlLinkedEnumBiDictionary<HbmCacheInclude> includeDict = new XmlLinkedEnumBiDictionary<HbmCacheInclude>();

        private HbmCache hbmCache;

        public HbmCacheConverter() : base(null)
        {
        }

        public override HbmCache Convert(CacheMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmCache;
        }

        public override void ProcessCache(CacheMapping cacheMapping)
        {
            hbmCache = new HbmCache();

            if (cacheMapping.IsSpecified("Region"))
                hbmCache.region = cacheMapping.Region;

            if (cacheMapping.IsSpecified("Usage"))
                hbmCache.usage = LookupEnumValueIn(usageDict, cacheMapping.Usage);

            if (cacheMapping.IsSpecified("Include"))
                hbmCache.include = LookupEnumValueIn(includeDict, cacheMapping.Include);
        }
    }
}
