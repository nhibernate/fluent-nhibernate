using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class Expectation<TInspector> : IExpectation
        where TInspector : IInspector
    {
        private readonly Expression<Func<TInspector, object>> expression;
        private readonly IAcceptanceCriterion value;

        public Expectation(Expression<Func<TInspector, object>> expression, IAcceptanceCriterion value)
        {
            this.expression = expression;
            this.value = value;
        }

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
}