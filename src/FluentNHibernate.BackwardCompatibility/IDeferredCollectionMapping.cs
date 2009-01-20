using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.BackwardCompatibility
{
    internal interface IDeferredCollectionMapping
    {
        ICollectionMapping ResolveCollectionMapping();
    }
}