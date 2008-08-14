using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.AutoMap
{
    public class ExpressionBuilder
    {
        public static Expression<Func<T, object>> Create<T>(PropertyInfo property)
        {
            var param = Expression.Parameter(typeof(T), "entity");
            var expression = Expression.Property(param, property);
            var castedProperty = Expression.Convert(expression, typeof(object));
            return (Expression<Func<T, object>>)Expression.Lambda(castedProperty, param);
        }
    }
}