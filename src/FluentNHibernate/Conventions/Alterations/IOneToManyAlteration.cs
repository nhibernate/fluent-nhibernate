namespace FluentNHibernate.Conventions.Alterations
{
    public interface IOneToManyAlteration
    {
        void KeyColumnName(string columnName);
    }
}