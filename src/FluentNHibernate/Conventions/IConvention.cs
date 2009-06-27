using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
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
        /// <param name="acceptance">Instace that could be supplied</param>
        /// <returns>Apply on this target?</returns>
        void Accept(IAcceptanceCriteria<TInspector> acceptance);
    }

    public interface IConventionApplier<TInstance>
        where TInstance : IInspector, IAlteration
    {
        /// <summary>
        /// Apply changes to the target
        /// </summary>
        void Apply(TInstance instance);
    }

    /// <summary>
    /// Basic convention interface. Don't use directly.
    /// </summary>
    /// <typeparam name="TInspector">Inspector instance for use in retrieving values and setting expectations</typeparam>
    /// <typeparam name="TAlteration">Alteration instance for altering the model</typeparam>
    /// <typeparam name="TInstance">Apply instance</typeparam>
    public interface IConvention<TInspector, TAlteration, TInstance>
        : IConvention,
          IConventionAcceptance<TInspector>,
          IConventionApplier<TInstance>
        where TInspector : IInspector
        where TAlteration : IAlteration
        where TInstance : TInspector, TAlteration
    {}
}