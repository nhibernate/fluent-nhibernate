using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public interface IOneToManyPart : ICollectionRelationship
    {
        new CollectionCascadeExpression<IOneToManyPart> Cascade { get; }
        new IOneToManyPart Inverse();
        new IOneToManyPart LazyLoad();
        INotFoundExpression NotFound { get; }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        new IOneToManyPart CollectionType<TCollection>();

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        new IOneToManyPart CollectionType(Type type);

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        new IOneToManyPart CollectionType(string type);

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        new IOneToManyPart Not { get; }
        IOneToManyPart KeyColumn(string columnName);
        IColumnNameCollection KeyColumns { get; }
        OuterJoinBuilder<IOneToManyPart> OuterJoin { get; }
        FetchTypeExpression<IOneToManyPart> Fetch { get; }
        OptimisticLockBuilder<IOneToManyPart> OptimisticLock { get; }
        IOneToManyPart Schema(string schema);
        IOneToManyPart Persister<T>() where T : IEntityPersister;
        IOneToManyPart Check(string checkSql);
        IOneToManyPart Generic();
        IOneToManyPart ForeignKeyConstraintName(string foreignKeyName);
    }
}
