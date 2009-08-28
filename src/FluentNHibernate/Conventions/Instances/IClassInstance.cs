using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IClassInstance : IClassInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IClassInstance Not { get; }
        new IOptimisticLockInstance OptimisticLock { get; }
        new ICacheInstance Cache { get; }
        new ISchemaActionInstance SchemaAction { get; }
        void Table(string tableName);
        void DynamicInsert();
        void DynamicUpdate();
        void BatchSize(int size);
        void LazyLoad();
        void ReadOnly();
        void Schema(string schema);
        void Where(string where);
        void Subselect(string subselectSql);
        void Proxy<T>();
        void Proxy(Type type);
        void Proxy(string type);
    }
}