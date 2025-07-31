using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class EqualCriterion(bool inverse, object value) : IAcceptanceCriterion
{
    public bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector) where T : IInspector
    {
        var func = propertyExpression.Compile();
        var actualValue = func(inspector);
        var result = actualValue.Equals(value);

        return (inverse) ? !result : result;
    }
}
