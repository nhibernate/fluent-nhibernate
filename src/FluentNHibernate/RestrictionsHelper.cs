using System;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Criterion;

namespace FluentNHibernate
{

	public static class RestrictionsHelper<MODEL>
	{
		
		public static AbstractCriterion Between(Expression<Func<MODEL, object>> propertyExpr, object low, object hi)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(propertyExpr);
			return Restrictions.Between(property.Name, low, hi);
		}
		
		public static SimpleExpression Eq(Expression<Func<MODEL, object>> propertyExpr, object value)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(propertyExpr);
			return Restrictions.Eq(property.Name, value);
		}
		
		public static AbstractCriterion EqProperty(Expression<Func<MODEL, object>> propertyExpr, Expression<Func<MODEL, object>> equalToPropertyExpr)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(propertyExpr);
			PropertyInfo equalToProperty = ReflectionHelper.GetProperty(equalToPropertyExpr);
			return Restrictions.EqProperty(property.Name, equalToProperty.Name);
		}
		
		public static AbstractCriterion IsNull(Expression<Func<MODEL, object>> propertyExpr)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(propertyExpr);
			return Restrictions.IsNull(property.Name);
		}
		
		public static AbstractCriterion IsNotNull(Expression<Func<MODEL, object>> propertyExpr)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(propertyExpr);
			return Restrictions.IsNotNull(property.Name);
		}
		
	}

}
