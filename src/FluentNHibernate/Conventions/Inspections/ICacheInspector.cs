namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICacheInspector : IInspector
    {
        string Usage { get; }
        string Region { get; }
        Include Include { get; }
    }
}