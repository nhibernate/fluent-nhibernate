using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IFilterMappingProvider
    {
        FilterMapping GetFilterMapping();
    }
}
