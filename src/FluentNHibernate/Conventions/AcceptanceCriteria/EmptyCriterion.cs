using System;
using System.Collections;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class EmptyCriterion(bool inverse) : IAcceptanceCriterion
{
    public bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector) where T : IInspector
    {
        var func = propertyExpression.Compile();
        var actualValue = func(inspector);

        if (!(actualValue is IEnumerable))
            return false;

        var result = ((IEnumerable)actualValue).GetEnumerator().Current is not null;

        return inverse ? !result : result;
    }
}
