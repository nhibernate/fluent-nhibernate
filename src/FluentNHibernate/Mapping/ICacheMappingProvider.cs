using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface ICacheMappingProvider
    {
        CacheMapping GetCacheMapping();
    }
}