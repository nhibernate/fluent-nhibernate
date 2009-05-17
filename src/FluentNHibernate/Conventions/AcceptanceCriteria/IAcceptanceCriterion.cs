using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.InspectionDsl;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public interface IAcceptanceCriterion
    {
        bool IsSatisfiedBy<T>(Expression<Func<T, object>> propertyExpression, T inspector)
            where T : IInspector;
    }
}