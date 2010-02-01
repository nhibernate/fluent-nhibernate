namespace FluentNHibernate.Conventions.Inspections
{
    public interface IBagInspector : ICollectionInspector
    {
        new string OrderBy { get; }
    }
}
