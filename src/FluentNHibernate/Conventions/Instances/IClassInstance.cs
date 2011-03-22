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

        /// <summary>
        /// Applies a filter to this entity given its name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        /// <param name="condition">The condition to apply</param>
        void ApplyFilter(string name, string condition);

        /// <summary>
        /// Applies a filter to this entity given its name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        void ApplyFilter(string name);

        /// <summary>
        /// Applies a named filter to this entity.
        /// </summary>
        /// <param name="condition">The condition to apply</param>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new();

        /// <summary>
        /// Applies a named filter to this entity.
        /// </summary>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new();
    }
}
