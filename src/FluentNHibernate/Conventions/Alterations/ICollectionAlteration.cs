namespace FluentNHibernate.Conventions.Alterations
{
    public interface ICollectionAlteration : IOneToManyCollectionAlteration, IManyToManyCollectionAlteration
    {
        new IKeyAlteration Key { get; }
        new void SetTableName(string tableName);
        new void Name(string name);
    }
}