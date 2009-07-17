namespace FluentNHibernate.Conventions.Inspections
{
    public interface IComponentBaseInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string ParentName { get; }
        bool Insert { get; }
        bool Update { get; }
    }

    public interface IComponentInspector : IComponentBaseInspector
    {}

    public interface IDynamicComponentInspector: IComponentBaseInspector
    {}
}
