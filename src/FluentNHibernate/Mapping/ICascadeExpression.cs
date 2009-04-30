namespace FluentNHibernate.Mapping
{
    public interface ICascadeExpression
    {
        void All();
        void None();
        void SaveUpdate();
        void Delete();
    }
}