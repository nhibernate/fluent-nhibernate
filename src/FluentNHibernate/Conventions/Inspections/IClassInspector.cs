namespace FluentNHibernate.Conventions.Inspections
{
    public interface IClassInspector : ILazyLoadInspector, IReadOnlyInspector
    {
        string TableName { get; }
        Cache Cache { get; }
        OptimisticLock OptimisticLock { get; }
        string Schema { get; }
        bool AutoImport { get; }
        bool DynamicUpdate { get; }
        bool DynamicInsert { get; }
        int BatchSize { get; }
    }
}