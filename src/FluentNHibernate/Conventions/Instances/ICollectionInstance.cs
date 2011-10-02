using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ICollectionInstance : ICollectionInspector
    {
        new IKeyInstance Key { get; }
        new IIndexInstanceBase Index { get; }
        new IElementInstance Element { get; }
        new IRelationshipInstance Relationship { get; }
        void Table(string tableName);
        new void Name(string name);
        new void Schema(string schema);
        new void LazyLoad();
        void ExtraLazyLoad();
        new void BatchSize(int batchSize);
        void ReadOnly();

        void AsArray();
        void AsBag();
        void AsList();
        void AsMap();
        void AsSet();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ICollectionInstance Not { get; }
        new IAccessInstance Access { get; }
        new ICacheInstance Cache { get; }
        new ICollectionCascadeInstance Cascade { get; }
        new IFetchInstance Fetch { get; }

        new void OptimisticLock();
        new void Check(string constraint);
        new void CollectionType<T>();
        new void CollectionType(string type);
        new void CollectionType(Type type);
        new void Generic();
        new void Inverse();
        new void Persister<T>();
        new void Where(string whereClause);
        new void OrderBy(string orderBy);
        new void Sort(string sort);
        void Subselect(string subselect);
        void KeyNullable();
    }
}