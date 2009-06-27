namespace FluentNHibernate.Conventions.Alterations
{
    public interface IManyToManyCollectionAlteration : IAlteration
    {
        IKeyAlteration Key { get; }
        IManyToManyAlteration ManyToMany { get; }
        void SetTableName(string tableName);
        void Name(string name);
    }
}