namespace FluentNHibernate.Conventions.Inspections;

public interface IExposedThroughPropertyInspector : IInspector
{
    Member Property { get; }
}
