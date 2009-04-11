using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.FluentInterface
{
    public interface IDeferredCollectionMapping
    {
        ICollectionMapping ResolveCollectionMapping();
    }
}