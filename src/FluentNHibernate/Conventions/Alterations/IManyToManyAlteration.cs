namespace FluentNHibernate.Conventions.Alterations
{
    public interface IManyToManyAlteration : IRelationshipAlteration
    {
        void ColumnName(string columnName);
    }
}