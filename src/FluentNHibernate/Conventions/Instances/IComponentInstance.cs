using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IComponentBaseInstance : IComponentBaseInspector
    {
        new IAccessInstance Access { get; }
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
