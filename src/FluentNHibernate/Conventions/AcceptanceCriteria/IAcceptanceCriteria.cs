using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.InspectionDsl;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IAcceptanceCriteria<TInspector>
        where TInspector : IInspector
    {
        IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, object>> propertyExpression, IAcceptanceCriterion value);
        IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, bool>> evaluation);
        IEnumerable<IExpectation> Expectations { get; }
        bool Matches(IInspector inspector);
    }
}