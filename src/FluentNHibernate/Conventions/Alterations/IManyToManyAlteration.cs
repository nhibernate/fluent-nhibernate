namespace FluentNHibernate.Conventions.Alterations
{
    public interface IManyToManyAlteration : IAlteration
    {
        void ColumnName(string columnName);
    }
}