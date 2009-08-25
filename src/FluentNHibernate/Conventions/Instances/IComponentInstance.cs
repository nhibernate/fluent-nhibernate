using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IComponentBaseInstance : IComponentBaseInspector
    {
        new IAccessInstance Access { get; }
        void Update();
        void Insert();
        void Unique();
        void OptimisticLock();
    }

    public interface IComponentInstance : IComponentBaseInstance, IComponentInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IComponentInstance Not { get; }
        void LazyLoad();
    }

    public interface IDynamicComponentInstance : IComponentBaseInstance, IDynamicComponentInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IDynamicComponentInstance Not { get; }
    }
}
