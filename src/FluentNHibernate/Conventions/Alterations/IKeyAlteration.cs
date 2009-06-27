namespace FluentNHibernate.Conventions.Alterations
{
    public interface IKeyAlteration : IAlteration
    {
        void ColumnName(string columnName);
    }
}