using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.FluentInterface
{
    internal interface IDeferredCollectionMapping
    {
        ICollectionMapping ResolveCollectionMapping();
    }
}