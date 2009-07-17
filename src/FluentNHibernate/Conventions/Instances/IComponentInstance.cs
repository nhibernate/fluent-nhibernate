using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IComponentBaseInstance : IComponentBaseInspector
    {}

    public interface IComponentInstance : IComponentBaseInstance, IComponentInspector
    {}
    
    public interface IDynamicComponentInstance : IComponentBaseInstance, IDynamicComponentInspector
    {}
}
