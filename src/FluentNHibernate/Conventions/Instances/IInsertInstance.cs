namespace FluentNHibernate.Conventions.Instances;

public interface IInsertInstance
{
    /// <summary>
    /// Specifies that the mapped columns should be included in or excluded from SQL INSERT statement.
    /// </summary>
    void Insert();
}
