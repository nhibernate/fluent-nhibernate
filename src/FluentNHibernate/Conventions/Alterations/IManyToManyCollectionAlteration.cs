namespace FluentNHibernate.Conventions.Alterations
{
    public interface IManyToManyCollectionAlteration : ICollectionAlteration
    {
        new IManyToManyAlteration Relationship { get; }
    }
}