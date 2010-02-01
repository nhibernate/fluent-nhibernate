namespace FluentNHibernate.Conventions.Inspections
{
    public interface ISetInspector : ICollectionInspector
    {
        new string OrderBy { get; }
        string Sort { get; }
    }
}
