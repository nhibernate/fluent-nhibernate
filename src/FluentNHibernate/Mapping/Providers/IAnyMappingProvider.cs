using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IAnyMappingProvider
    {
        AnyMapping GetAnyMapping();
    }
}