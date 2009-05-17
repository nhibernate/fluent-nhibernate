namespace FluentNHibernate.Conventions.Alterations
{
    public interface IClassAlteration
    {
        void WithTable(string tableName);
    }
}