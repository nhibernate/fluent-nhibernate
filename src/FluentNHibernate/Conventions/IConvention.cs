namespace FluentNHibernate.Conventions
{
    public interface IConvention<T>
    {
        bool Accept(T target);
        void Apply(T target, ConventionOverrides overrides);
    }
}