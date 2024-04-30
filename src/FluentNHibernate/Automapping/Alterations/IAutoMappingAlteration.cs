#if USE_NULLABLE
#nullable enable
#endif
namespace FluentNHibernate.Automapping.Alterations;

/// <summary>
/// Provides a mechanism for altering an AutoPersistenceModel prior to
/// the generation of mappings.
/// </summary>
public interface IAutoMappingAlteration
{
    /// <summary>
    /// Alter the model
    /// </summary>
    /// <param name="model">AutoPersistenceModel instance to alter</param>
    void Alter(AutoPersistenceModel model);
}
