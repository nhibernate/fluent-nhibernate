namespace FluentNHibernate.Conventions.Inspections
{
    public interface IComponentBaseInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string ParentName { get; }
        bool Insert();
        bool Update();
    }

    public interface IComponentInspector : IComponentBaseInspector
    {}

    public interface IDynamicComponentInspector: IComponentBaseInspector
    {}
}
