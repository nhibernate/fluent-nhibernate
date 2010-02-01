using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IComponentBaseInstance : IComponentBaseInspector
    {
        new IAccessInstance Access { get; }
        new void Update();
        new void Insert();
        new void Unique();
        new void OptimisticLock();
    }

    public interface IComponentInstance : IComponentBaseInstance, IComponentInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IComponentInstance Not { get; }
        new void LazyLoad();
    }

    public interface IDynamicComponentInstance : IComponentBaseInstance, IDynamicComponentInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IDynamicComponentInstance Not { get; }
    }
}
