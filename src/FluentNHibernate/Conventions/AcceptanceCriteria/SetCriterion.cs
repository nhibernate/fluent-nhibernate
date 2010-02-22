using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class SetCriterion : IAcceptanceCriterion
    {
        private readonly bool inverse;

        public SetCriterion(bool inverse)
        {
            this.inverse = inverse;
        }

        public bool IsSatisfiedBy<T>(Expression<Func<T, object>> expression, T inspector) where T : IInspector
        {
            var member = expression.ToMember();
            var result = inspector.IsSet(member);

            return inverse ? !result : result;
        }
    }
}