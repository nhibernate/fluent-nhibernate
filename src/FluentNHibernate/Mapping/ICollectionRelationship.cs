using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.Mapping
{
    public interface ICollectionRelationship : IRelationship
    {
        bool IsMethodAccess { get; }
        MemberInfo Member { get; }
        string TableName { get; }
        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        ICache Cache { get; }
        ICollectionCascadeExpression Cascade { get; }
        
        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ICollectionRelationship Not { get; }
        ICollectionRelationship LazyLoad();
        ICollectionRelationship Inverse();
        ICollectionRelationship AsSet();
        ICollectionRelationship AsBag();
        ICollectionRelationship AsList();
        ICollectionRelationship AsMap(string indexColumnName);
        ICollectionRelationship AsMap<INDEX_TYPE>(string indexColumnName);
        ICollectionRelationship AsElement(string columnName);

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        ICollectionRelationship Component(Action<IClasslike> action);

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        ICollectionRelationship WithTableName(string name);

        ICollectionRelationship WithForeignKeyConstraintName(string foreignKeyName);
        ICollectionRelationship ForeignKeyCascadeOnDelete();



        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// </summary>
        ICollectionRelationship Where(string where);

        ICollectionRelationship BatchSize(int size);

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship CollectionType<TCollection>();

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship CollectionType(Type type);

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship CollectionType(string type);
    }
}