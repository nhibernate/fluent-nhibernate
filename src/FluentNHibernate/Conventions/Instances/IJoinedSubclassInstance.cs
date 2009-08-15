using System;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IJoinedSubclassInstance : IJoinedSubclassInspector
    {
        new IKeyInstance Key { get; }
        IJoinedSubclassInstance Not { get; }
        void Abstract();
        void Check(string constraint);
        void DynamicInsert();
        void DynamicUpdate();
        void LazyLoad();
        void Proxy(Type type);
        void Proxy<T>();
        void Schema(string schema);
        void SelectBeforeUpdate();
        void Table(string tableName);
        void Subselect(string subselect);
        void Persister<T>() where T : IEntityPersister;
        void Persister(Type type);
        void Persister(string type);
        void BatchSize(int batchSize);
    }
 }