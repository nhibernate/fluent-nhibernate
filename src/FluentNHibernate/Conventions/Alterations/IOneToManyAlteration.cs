namespace FluentNHibernate.Conventions.Alterations
{
    public interface IOneToManyAlteration
    {
    }

    public interface IOneToManyCollectionAlteration
    {
        IKeyAlteration Key { get; }
        IOneToManyAlteration OneToMany { get; }
    }

    public interface IManyToManyCollectionAlteration
    {
        IKeyAlteration Key { get; }
        IManyToManyAlteration ManyToMany { get; }
    }

    public interface ICollectionAlteration : IOneToManyCollectionAlteration, IManyToManyCollectionAlteration
    {
        new IKeyAlteration Key { get; }
        void TableName(string tableName);
    }

    public interface IKeyAlteration
    {
        void ColumnName(string columnName);
    }
}