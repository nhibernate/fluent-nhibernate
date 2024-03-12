using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

[Multiple]
public abstract class BuiltConventionBase<TInspector, TInstance>(Action<IAcceptanceCriteria<TInspector>> accept, Action<TInstance> convention)
    where TInspector : IInspector
{
    public void Accept(IAcceptanceCriteria<TInspector> acceptance)
    {
        accept(acceptance);
    }

    public void Apply(TInstance instance)
    {
        convention(instance);
    }
}
