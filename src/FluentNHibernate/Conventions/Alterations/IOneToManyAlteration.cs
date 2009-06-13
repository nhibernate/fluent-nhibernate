namespace FluentNHibernate.Conventions.Alterations
{
    public interface IOneToManyAlteration
    {
    }

    public interface IOneToManyCollectionAlteration
    {
        IKeyAlteration Key { get; }
        IOneToManyAlteration OneToMany { get; }
        void TableName(string tableName);
        void Name(string name);
    }

    public interface IManyToManyCollectionAlteration
    {
        IKeyAlteration Key { get; }
        IManyToManyAlteration ManyToMany { get; }
        void TableName(string tableName);
        void Name(string name);
    }

    public interface ICollectionAlteration : IOneToManyCollectionAlteration, IManyToManyCollectionAlteration
    {
        new IKeyAlteration Key { get; }
        new void TableName(string tableName);
        new void Name(string name);
    }

    public interface IKeyAlteration
    {
        void ColumnName(string columnName);
    }
}