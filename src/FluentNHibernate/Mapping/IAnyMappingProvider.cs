using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IAnyMappingProvider
    {
        AnyMapping GetAnyMapping();
    }
}