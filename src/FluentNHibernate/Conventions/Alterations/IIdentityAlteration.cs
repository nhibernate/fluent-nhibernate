namespace FluentNHibernate.Conventions.Alterations
{
    public interface IIdentityAlteration : IAlteration
    {
        void ColumnName(string column);
    }
}