using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Providers
{
    public interface ICollectionMappingProvider
    {
        MappingModel.Collections.CollectionMapping GetCollectionMapping();
    }
}