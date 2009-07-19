using System;
using System.Collections;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public interface ICollectionRelationship : IRelationship
    {
        bool IsMethodAccess { get; }
        MemberInfo Member { get; }
        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        CachePart Cache { get; }
        ICollectionCascadeExpression Cascade { get; }
        
        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ICollectionRelationship Not { get; }
        ICollectionRelationship LazyLoad();
        ICollectionRelationship Inverse();
        ICollectionRelationship AsSet();
        ICollectionRelationship AsSet(SortType sort);
        ICollectionRelationship AsSet<TComparer>() where TComparer : IComparer;
        ICollectionRelationship AsBag();
        ICollectionRelationship AsList();
        ICollectionRelationship AsMap(string indexColumnName);
        ICollectionRelationship AsMap(string indexColumnName, SortType sort);
        ICollectionRelationship AsMap<TIndex>(string indexColumnName);
        ICollectionRelationship AsMap<TIndex>(string indexColumnName, SortType sort);
        ICollectionRelationship AsMap<TIndex, TComparer>(string indexColumnName) where TComparer : IComparer;
        ICollectionRelationship Element(string columnName);

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        ICollectionRelationship Table(string name);

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

        ICollectionMapping GetCollectionMapping();
    }
}