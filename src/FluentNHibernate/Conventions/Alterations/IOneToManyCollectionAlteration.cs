namespace FluentNHibernate.Conventions.Alterations
{
    public interface IOneToManyCollectionAlteration : IAlteration
    {
        IKeyAlteration Key { get; }
        IOneToManyAlteration OneToMany { get; }
        void SetTableName(string tableName);
        void Name(string name);
    }
}