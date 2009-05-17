namespace FluentNHibernate.Conventions.InspectionDsl
{
    public interface IReadOnlyInspector : IInspector
    {
        bool ReadOnly { get; }
    }
}