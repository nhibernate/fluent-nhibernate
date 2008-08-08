namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Dictates whether a element can have attributes set on it.
    /// </summary>
    public interface IHasAttributes
    {
        /// <summary>
        /// Set an attribute on the xml element produced by this mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        void SetAttribute(string name, string value);
    }
}