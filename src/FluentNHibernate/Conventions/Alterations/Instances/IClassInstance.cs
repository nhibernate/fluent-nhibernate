using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IClassInstance : IClassInspector, IClassAlteration
    {
        new IOptimisticLockBuilder OptimisticLock { get; }
    }
}