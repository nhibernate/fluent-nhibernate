using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class InverterAcceptanceCriteria<TInspector> : ConcreteAcceptanceCriteria<TInspector>
        where TInspector : IInspector
    {
        protected override IExpectation CreateExpectation(Expression<Func<TInspector, object>> expression, IAcceptanceCriterion value)
        {
            var expectation = base.CreateExpectation(expression, value);

            return new InvertedExpectation(expectation);
        }

        protected override IExpectation CreateEvalExpectation(Expression<Func<TInspector, bool>> expression)
        {
            var expectation = base.CreateEvalExpectation(expression);

            return new InvertedExpectation(expectation);
        }

        protected override IExpectation CreateCollectionExpectation<TCollectionItem>(Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property, ICollectionAcceptanceCriterion<TCollectionItem> value)
        {
            var expectation = base.CreateCollectionExpectation<TCollectionItem>(property, value);
            
            return new InvertedExpectation(expectation);
        }
    }
}