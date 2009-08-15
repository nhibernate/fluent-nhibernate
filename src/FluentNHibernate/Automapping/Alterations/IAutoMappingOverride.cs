namespace FluentNHibernate.Automapping.Alterations
{
    /// <summary>
    /// A mapping override for an auto mapped entity.
    /// </summary>
    /// <typeparam name="T">Entity who's auto-mapping you're overriding</typeparam>
    public interface IAutoMappingOverride<T>
    {
        /// <summary>
        /// Alter the automapping for this type
        /// </summary>
        /// <param name="mapping">Automapping</param>
        void Override(AutoMapping<T> mapping);
    }
}
