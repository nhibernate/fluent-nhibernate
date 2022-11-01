namespace FluentNHibernate.Conventions.Instances;

public interface IUpdateInstance
{
    /// Specifies that the mapped columns should be included or excluded in SQL UPDATE statement.
    void Update();
}
