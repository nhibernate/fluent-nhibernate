using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping.Providers
{
    public interface ICompositeIdMappingProvider
    {
        CompositeIdMapping GetCompositeIdMapping();
    }
}