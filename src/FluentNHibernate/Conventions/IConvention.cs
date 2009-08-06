using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Ignore - this is used for generic restrictions only
    /// </summary>
    public interface IConvention
    {}

    public interface IConventionAcceptance<TInspector>
        where TInspector: IInspector
    {
        /// <summary>
        /// Whether this convention will be applied to the target.
        /// </summary>
        /// <param name="criteria">Instace that could be supplied</param>
        /// <returns>Apply on this target?</returns>
        void Accept(IAcceptanceCriteria<TInspector> criteria);
    }

    /// <summary>
    /// Basic convention interface. Don't use directly.
    /// </summary>
    /// <typeparam name="TInspector">Inspector instance for use in retrieving values and setting expectations</typeparam>
    /// <typeparam name="TInstance">Apply instance</typeparam>
    public interface IConvention<TInspector, TInstance>
        : IConvention
        where TInspector : IInspector
        where TInstance : TInspector
    {
        /// <summary>
        /// Apply changes to the target
        /// </summary>
        void Apply(TInstance instance);
    }
}