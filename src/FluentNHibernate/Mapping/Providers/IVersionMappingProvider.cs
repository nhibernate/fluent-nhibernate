using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IVersionMappingProvider
    {
        VersionMapping GetVersionMapping();
    }
}