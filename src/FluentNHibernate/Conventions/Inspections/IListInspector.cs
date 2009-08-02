namespace FluentNHibernate.Conventions.Inspections
{
    public interface IListInspector : ICollectionInspector
    {
        IIndexInspectorBase Index { get; } 
    }
}
