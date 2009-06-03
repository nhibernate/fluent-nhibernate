namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Denotes that a mapping part has an access strategy.
    /// </summary>
    /// <typeparam name="T">Parent element, property, many-to-one etc...</typeparam>
    public interface IAccessStrategy<T>
    {
        /// <summary>
        /// Set the access and naming strategy for this element.
        /// </summary>
        AccessStrategyBuilder<T> Access { get; }
    }
}