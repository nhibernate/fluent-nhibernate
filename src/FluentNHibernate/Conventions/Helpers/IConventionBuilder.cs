using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public interface IConventionBuilder<TConvention, TInspector, TAlteration, TInstance>
        where TConvention : IConvention<TInspector, TAlteration, TInstance>
        where TInspector : IInspector
        where TAlteration : IAlteration
        where TInstance : TInspector, TAlteration
    {
        TConvention Always(Action<TInstance> convention);
        TConvention When(Action<IAcceptanceCriteria<TInspector>> expectations, Action<TInstance> convention);
    }
}