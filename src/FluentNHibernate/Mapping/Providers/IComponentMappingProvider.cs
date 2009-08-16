using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IComponentMappingProvider
    {
        IComponentMapping GetComponentMapping();
    }
}