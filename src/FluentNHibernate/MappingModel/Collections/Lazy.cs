namespace FluentNHibernate.MappingModel.Collections
{
    /// <summary>
    /// Determines the lazy-loading strategy for a collection mapping.
    /// </summary>
    public enum Lazy
    {
        /// <summary>
        /// Collection will be eager loaded (lazy=false).
        /// </summary>
        False,
        /// <summary>
        /// Collection will lazy loaded (lazy=true).
        /// </summary>
        True,
        /// <summary>
        /// collection will be extra lazy loaded (lazy=extra).
        /// </summary>
        /// <remarks>
        /// "Extra" lazy collections are mostly similar to lazy=true, except certain operations on the collection will not load the whol collection
        /// but issue a smarter SQL statement. For example, invoking Count on an extra-lazy collection will issue a "SELECT COUNT(*)..." rather than selecting 
        /// and loading the whole collection of entities.
        /// </remarks>
        Extra
    }
}