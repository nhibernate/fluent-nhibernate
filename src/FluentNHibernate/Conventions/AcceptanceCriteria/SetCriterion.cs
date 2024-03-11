using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Conventions.AcceptanceCriteria;

public class SetCriterion(bool inverse) : IAcceptanceCriterion
{
    public bool IsSatisfiedBy<T>(Expression<Func<T, object>> expression, T inspector) where T : IInspector
    {
        var member = expression.ToMember();
        var result = inspector.IsSet(member);

        return inverse ? !result : result;
    }
}
