using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Providers;

public interface ICollectionIdMappingProvider
{
    CollectionIdMapping GetCollectionIdMapping();
}
