using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface ICacheMappingProvider
    {
        CacheMapping GetCacheMapping();
    }
}