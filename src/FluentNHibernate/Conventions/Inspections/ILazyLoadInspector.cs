namespace FluentNHibernate.Conventions.Inspections;

public interface ILazyLoadInspector : IInspector
{
    bool LazyLoad { get; }
}
