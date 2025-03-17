namespace FluentNHibernate.Conventions.Instances;

public interface ICascadeInstance
{
    void All();
    void None();
    void SaveUpdate();
    void Delete();
    void Merge();
}
