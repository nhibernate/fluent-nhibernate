using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public interface IConventionBuilder<TConvention, TInspector, TInstance>
        where TConvention : IConvention<TInspector, TInstance>
        where TInspector : IInspector
        where TInstance : TInspector
    {
        TConvention Always(Action<TInstance> convention);
        TConvention When(Action<IAcceptanceCriteria<TInspector>> expectations, Action<TInstance> convention);
    }
}