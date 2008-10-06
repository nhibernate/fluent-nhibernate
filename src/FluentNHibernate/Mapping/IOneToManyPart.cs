using System;

namespace FluentNHibernate.Mapping
{
    public interface IOneToManyPart : IMappingPart
    {
        CollectionCascadeExpression<IOneToManyPart> Cascade { get; }
        IOneToManyPart IsInverse();
        IOneToManyPart LazyLoad();
    }
}
