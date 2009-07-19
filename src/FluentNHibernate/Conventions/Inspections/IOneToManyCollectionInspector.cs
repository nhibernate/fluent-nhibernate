namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToManyCollectionInspector : ICollectionInspector
    {
        new IOneToManyInspector Relationship { get; }
    }
}