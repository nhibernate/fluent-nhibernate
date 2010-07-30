using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentNHibernate
{
    public static class Reveal
    {
        [Obsolete("Use Reveal.Member")]
        public static Expression<Func<TEntity, object>> Property<TEntity>(string propertyName)
        {
            return Member<TEntity>(propertyName);
        }

        [Obsolete("Use Reveal.Member")]
        public static Expression<Func<TEntity, TReturn>> Property<TEntity, TReturn>(string propertyName)
        {
            return Member<TEntity, TReturn>(propertyName);
        }

        /// <summary>
        /// Reveals a hidden property or field for use instead of expressions.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="name">Name of property or field</param>
        /// <returns>Expression for the hidden property or field</returns>
        public static Expression<Func<TEntity, object>> Member<TEntity>(string name)
        {
            return CreateExpression<TEntity, object>(name);
        }

        /// <summary>
        /// Reveals a hidden property or field with a specific return type for use instead of expressions.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TReturn">Property or field return type</typeparam>
        /// <param name="name">Name of property or field</param>
        /// <returns>Expression for the hidden property or field</returns>
        public static Expression<Func<TEntity, TReturn>> Member<TEntity, TReturn>(string name)
        {
            return CreateExpression<TEntity, TReturn>(name);
        }

        static Expression<Func<TEntity, TReturn>> CreateExpression<TEntity, TReturn>(string propertyName)
        {
            var type = typeof(TEntity);
            var member = type.GetInstanceMembers()
                .FirstOrDefault(x => x.Name == propertyName);

            if (member == null)
                throw new UnknownPropertyException(type, propertyName);

            var param = Expression.Parameter(member.DeclaringType, "x");
            Expression expression = Expression.PropertyOrField(param, propertyName);

            if (member.PropertyType.IsValueType)
                expression = Expression.Convert(expression, typeof(object));

            return (Expression<Func<TEntity, TReturn>>)Expression.Lambda(typeof(Func<TEntity, TReturn>), expression, param);
        }
    }
}