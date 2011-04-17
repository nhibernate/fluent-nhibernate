namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyCollectionInspector : ICollectionInspector
    {
        new IManyToManyInspector Relationship { get; }
        IManyToManyCollectionInspector OtherSide { get; }
    }
}