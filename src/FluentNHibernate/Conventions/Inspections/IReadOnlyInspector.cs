namespace FluentNHibernate.Conventions.Inspections
{
    public interface IReadOnlyInspector : IInspector
    {
        bool ReadOnly { get; }
    }
}