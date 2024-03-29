using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class Expectation<TInspector>(Expression<Func<TInspector, object>> expression, IAcceptanceCriterion value)
    : IExpectation
    where TInspector : IInspector
{
    public bool Matches(TInspector inspector)
    {
        return value.IsSatisfiedBy(expression, inspector);
    }

    bool IExpectation.Matches(IInspector inspector)
    {
        if (inspector is TInspector)
            return Matches((TInspector)inspector);

        return false;
    }
}
