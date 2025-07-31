using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class EvalExpectation<TInspector>(Func<TInspector, bool> expression) : IExpectation
    where TInspector : IInspector
{
    public bool Matches(TInspector inspector)
    {
        return expression(inspector);
    }

    bool IExpectation.Matches(IInspector inspector)
    {
        if (inspector is TInspector)
            return Matches((TInspector)inspector);

        return false;
    }
}
