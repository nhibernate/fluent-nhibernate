namespace FluentNHibernate.Conventions.Alterations
{
    public interface IOneToManyCollectionAlteration : ICollectionAlteration
    {
        new IOneToManyAlteration Relationship { get; }
    }
}