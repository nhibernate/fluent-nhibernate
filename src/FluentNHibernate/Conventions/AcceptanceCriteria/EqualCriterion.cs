using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class EqualCriterion : IAcceptanceCriterion
    {
        private readonly bool inverse;
        private readonly object value;

        public EqualCriterion(bool inverse, object value)
        {
            this.inverse = inverse;
            this.value = value;
        }

        public bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector) where T : IInspector
        {
            var func = propertyExpression.Compile();
            var actualValue = func(inspector);
            var result = actualValue.Equals(value);

            return (inverse) ? !result : result;
        }
    }
}