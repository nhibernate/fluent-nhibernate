namespace FluentNHibernate.Conventions.Inspections
{
    public interface IBagInspector : ICollectionInspector
    {
        string OrderBy { get; }
    }
}
