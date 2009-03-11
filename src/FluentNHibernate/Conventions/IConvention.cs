namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Ignore - this is used for generic restrictions only
    /// </summary>
    public interface IConvention
    {}

    public interface IConvention<T> : IConvention
    {
        bool Accept(T target);
        void Apply(T target);
    }
}