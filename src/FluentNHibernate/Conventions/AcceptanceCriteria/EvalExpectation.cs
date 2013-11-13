using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class EvalExpectation<TInspector> : IExpectation
        where TInspector : IInspector
    {
        private readonly Func<TInspector, bool> expression;

        public EvalExpectation(Func<TInspector, bool> expression)
        {
            this.expression = expression;
        }

        public bool Matches(TInspector inspector)
        {
            return expression(inspector);
        }

        bool IExpectation.Matches(IInspector inspector)
        {
            if (inspector is TInspector)
                return Matches((TInspector)inspector);

            return false;
        }
    }
}