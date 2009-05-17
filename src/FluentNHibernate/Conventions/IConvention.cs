using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;

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
    /// <typeparam name="TInspector">Inspector instance for use in retrieving values and setting expectations</typeparam>
    /// <typeparam name="TAlteration">Alteration instance for altering the model</typeparam>
    public interface IConvention<TInspector, TAlteration> : IConvention
        where TInspector : IInspector
    {
        /// <summary>
        /// Whether this convention will be applied to the target.
        /// </summary>
        /// <param name="target">Instace that could be supplied</param>
        /// <returns>Apply on this target?</returns>
        void Accept(IAcceptanceCriteria<TInspector> target);

        /// <summary>
        /// Apply changes to the target
        /// </summary>
        /// <param name="alteration">Instance to alter</param>
        /// <param name="inspector">Inspector to retrieve values from if needed</param>
        void Apply(TAlteration alteration, TInspector inspector);
    }
}