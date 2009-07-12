namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICacheInspector
    {
        string Value { get; }
        string RegionValue { get; }
    }
}