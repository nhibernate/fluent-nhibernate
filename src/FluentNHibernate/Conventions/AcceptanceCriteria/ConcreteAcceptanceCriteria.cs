using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.InspectionDsl;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class ConcreteAcceptanceCriteria<TInspector> : IAcceptanceCriteria<TInspector>
        where TInspector : IInspector
    {
        private readonly List<IExpectation> expectations = new List<IExpectation>();

        public IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, object>> propertyExpression, IAcceptanceCriterion value)
        {
            expectations.Add(new Expectation<TInspector>(propertyExpression, value));
            return this;
        }

        public IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, bool>> evaluation)
        {
            expectations.Add(new EvalExpectation<TInspector>(evaluation));
            return this;
        }

        public IEnumerable<IExpectation> Expectations
        {
            get { return expectations; }
        }

        public bool Matches(IInspector inspector)
        {
            return Expectations.All(x => x.Matches(inspector));
        }
    }
}