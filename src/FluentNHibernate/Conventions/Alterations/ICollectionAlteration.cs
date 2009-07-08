namespace FluentNHibernate.Conventions.Alterations
{
    public interface ICollectionAlteration : IAlteration
    {
        IKeyAlteration Key { get; }
        IRelationshipAlteration Relationship { get; }
        void SetTableName(string tableName);
        void Name(string name);
    }
 }