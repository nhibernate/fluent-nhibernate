using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IAcceptanceCriterion
    {
        bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector)
            where T : IInspector;
    }

    public interface ICollectionAcceptanceCriterion<TCollectionItem>
    {
        bool IsSatisfiedBy<T>(Expression<Func<T, IEnumerable<TCollectionItem>>> propertyExpression, T inspector)
            where T : IInspector;
    }
}