namespace FluentNHibernate.Conventions.InspectionDsl
{
    public interface ILazyLoadInspector : IInspector
    {
        bool LazyLoad { get; }
    }
}