namespace FluentNHibernate.Conventions.Inspections
{
    public interface IArrayInspector : ICollectionInspector
    {
        IIndexInspectorBase Index { get; } 
    }
}
