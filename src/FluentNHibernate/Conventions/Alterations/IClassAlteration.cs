using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Alterations
{
    public interface IClassAlteration : IAlteration
    {
        void WithTable(string tableName);
        void DynamicInsert();
        void DynamicUpdate();
        IClassAlteration Not { get; }
        IOptimisticLockBuilder OptimisticLock { get; }
    }
}