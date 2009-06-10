namespace FluentNHibernate.Conventions.Alterations
{
    public interface IManyToManyAlteration
    {
        void ParentKeyColumn(string columnName);
        void ChildKeyColumn(string columnName);
    }
}