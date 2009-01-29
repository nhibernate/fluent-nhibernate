using System;

namespace FluentNHibernate.Mapping
{
    public interface IOneToManyPart : IMappingPart
    {
        CollectionCascadeExpression<IOneToManyPart> Cascade { get; }
        IOneToManyPart Inverse();
        IOneToManyPart LazyLoad();

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        IOneToManyPart Not { get; }
    }
}
