namespace FluentNHibernate.Conventions.Alterations
{
    public interface IClassAlteration : IAlteration
    {
        void WithTable(string tableName);
    }
}