namespace FluentNHibernate.Conventions.Inspections
{
    public interface IComponentInspector : IComponentBaseInspector
    {
        bool LazyLoad { get; }
    }
}
