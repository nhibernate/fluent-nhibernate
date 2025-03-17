using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class CollectionExpectation<TInspector, TCollectionItem>(
    Expression<Func<TInspector, IEnumerable<TCollectionItem>>> expression,
    ICollectionAcceptanceCriterion<TCollectionItem> criterion)
    : IExpectation
    where TInspector : IInspector
{
    public bool Matches(TInspector inspector)
    {
        return criterion.IsSatisfiedBy(expression, inspector);
    }

    bool IExpectation.Matches(IInspector inspector)
    {
        if (inspector is TInspector)
            return Matches((TInspector)inspector);

        return false;
    }
}
