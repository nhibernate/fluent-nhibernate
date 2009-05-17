using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class EvalExpectation<TInspector> : IExpectation
        where TInspector : IInspector
    {
        private readonly Expression<Func<TInspector, bool>> expression;

        public EvalExpectation(Expression<Func<TInspector, bool>> expression)
        {
            this.expression = expression;
        }

        public bool Matches(TInspector inspector)
        {
            var compiledFunc = expression.Compile();

            return compiledFunc(inspector);
        }

        bool IExpectation.Matches(IInspector inspector)
        {
            if (inspector is TInspector)
                return Matches((TInspector)inspector);

            return false;
        }
    }
}