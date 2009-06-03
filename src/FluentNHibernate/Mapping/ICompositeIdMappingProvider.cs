using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public interface ICompositeIdMappingProvider
    {
        CompositeIdMapping GetCompositeIdMapping();
    }
}