namespace FluentNHibernate.Conventions.Instances;

/// <summary>
/// Instructs NHibernate to immediately issues a SELECT after INSERT or UPDATE to retrieve the generated values.
/// <para> See https://nhibernate.info/doc/nhibernate-reference/mapping.html#mapping-generated for more information. </para>
/// </summary>
/// <remarks> It is user's responsibility to specify how the columns are generated. </remarks>
public interface IGeneratedInstance
{
    /// <summary>
    /// Marks the property value as not generated within the database (default).
    /// </summary>
    void Never();

    /// <summary>
    /// Marks the property value as generated on INSERT, but is not regenerated on subsequent UPDATEs.
    /// <para>NHibernate will immediately issues a SELECT after INSERT to retrieve the generated values.</para>
    /// </summary>
    void Insert();

    /// <summary>
    /// Marks the property value as generated both on INSERT and on UPDATE.
    /// <para>NHibernate will immediately issues a SELECT after INSERT or UPDATE to retrieve the generated values.</para>
    /// </summary>
    void Always();
}
