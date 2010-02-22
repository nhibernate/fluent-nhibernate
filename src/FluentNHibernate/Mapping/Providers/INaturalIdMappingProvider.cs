using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface INaturalIdMappingProvider
    {
        NaturalIdMapping GetNaturalIdMapping();
    }
}
