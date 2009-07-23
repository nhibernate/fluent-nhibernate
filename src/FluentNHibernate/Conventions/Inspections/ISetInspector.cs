namespace FluentNHibernate.Conventions.Inspections
{
    public interface ISetInspector : ICollectionInspector
    {
        string OrderBy { get; }
        string Sort { get; }
    }
}
