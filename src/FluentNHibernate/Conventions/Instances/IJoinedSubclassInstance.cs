using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IJoinedSubclassInstance : IJoinedSubclassInspector
    {
        new IKeyInstance Key { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IJoinedSubclassInstance Not { get; }
        new void Abstract();
        new void Check(string constraint);
        new void DynamicInsert();
        new void DynamicUpdate();
        new void LazyLoad();
        new void Proxy(Type type);
        new void Proxy<T>();
        void Schema(string schema);
        new void SelectBeforeUpdate();
        void Table(string tableName);
        void Subselect(string subselect);
        void Persister<T>() where T : IEntityPersister;
        void Persister(Type type);
        void Persister(string type);
        void BatchSize(int batchSize);
    }
 }