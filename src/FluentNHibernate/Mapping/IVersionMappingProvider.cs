using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IVersionMappingProvider
    {
        VersionMapping GetVersionMapping();
    }
}