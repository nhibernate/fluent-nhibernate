using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate
{
    public class Reveal
    {
        /// <summary>
        /// Reveals a hidden property for use instead of expressions.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Expression for the hidden property</returns>
        public static Expression<Func<TEntity, object>> Property<TEntity>(string propertyName)
        {
            return CreateExpression<TEntity, object>(propertyName);
        }

        /// <summary>
        /// Reveals a hidden property with a specific return type for use instead of expressions.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TReturn">Property return type</typeparam>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Expression for the hidden property</returns>
        public static Expression<Func<TEntity, TReturn>> Property<TEntity, TReturn>(string propertyName)
        {
            return CreateExpression<TEntity, TReturn>(propertyName);
        }

        private static Expression<Func<TEntity, TReturn>> CreateExpression<TEntity, TReturn>(string propertyName)
        {
            var type = typeof(TEntity);
            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (property == null)
                throw new UnknownPropertyException(type, propertyName);

            var param = Expression.Parameter(type, "x");
            Expression expression = Expression.Property(param, property);

            if (property.PropertyType.IsValueType)
                expression = Expression.Convert(expression, typeof(object));

            return (Expression<Func<TEntity, TReturn>>)Expression.Lambda(typeof(Func<TEntity, TReturn>), expression, param);
        }
    }
}