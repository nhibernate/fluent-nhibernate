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
        new void DynamicInsert();
        new void DynamicUpdate();
        new void BatchSize(int size);
        new void LazyLoad();
        new void ReadOnly();
        new void Schema(string schema);
        new void Where(string where);
        new void Subselect(string subselectSql);
        new void Proxy<T>();
        new void Proxy(Type type);
        new void Proxy(string type);
		void EntityName(string name);
	}
}
