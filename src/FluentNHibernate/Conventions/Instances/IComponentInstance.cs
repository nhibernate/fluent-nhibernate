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
        IComponentInstance Not { get; }
    }

    public interface IDynamicComponentInstance : IComponentBaseInstance, IDynamicComponentInspector
    {
        IDynamicComponentInstance Not { get; }
    }
}
