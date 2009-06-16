using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public interface IConventionBuilder<TConvention, TInspector, TAlteration>
        where TConvention : IConvention<TInspector, TAlteration>
        where TInspector : IInspector
    {
        TConvention Always(Action<TAlteration> convention);
        TConvention Always(Action<TAlteration, TInspector> convention);
        TConvention When(Action<IAcceptanceCriteria<TInspector>> expectations, Action<TAlteration> convention);
        TConvention When(Action<IAcceptanceCriteria<TInspector>> expectations, Action<TAlteration, TInspector> convention);
    }
}