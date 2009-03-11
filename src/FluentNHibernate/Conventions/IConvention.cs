namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Ignore - this is used for generic restrictions only
    /// </summary>
    public interface IConvention
    {}

    /// <summary>
    /// Basic convention interface. Don't use directly.
    /// </summary>
    /// <typeparam name="T">Mapping to apply conventions to</typeparam>
    public interface IConvention<T> : IConvention
    {
        /// <summary>
        /// Whether this convention will be applied to the target.
        /// </summary>
        /// <param name="target">Instace that could be supplied</param>
        /// <returns>Apply on this target?</returns>
        bool Accept(T target);

        /// <summary>
        /// Apply changes to the target
        /// </summary>
        /// <param name="target">Instance to apply changes to</param>
        void Apply(T target);
    }
}