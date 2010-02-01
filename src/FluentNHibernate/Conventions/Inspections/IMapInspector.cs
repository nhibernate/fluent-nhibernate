namespace FluentNHibernate.Conventions.Inspections
{
    public interface IMapInspector : ICollectionInspector
    {
        IIndexInspectorBase Index { get; }
        string Sort { get; }
    }
}
