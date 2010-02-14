using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Utils.Reflection
{
    public class ExpressionBuilder
    {
        public static Expression<Func<T, object>> Create<T>(Member member)
        {
            if (member is PropertyMember)
                return Create<T>((PropertyInfo)member.MemberInfo);

            throw new InvalidOperationException("Cannot create property expression from non-property Member.");
        }

        public static Expression<Func<T, object>> Create<T>(PropertyInfo property)
        {
            return (Expression<Func<T, object>>)Create(property, typeof(T));
        }

        public static object Create(PropertyInfo property, Type type)
        {
            var param = Expression.Parameter(type, "entity");
            var expression = Expression.Property(param, property);
            var castedProperty = Expression.Convert(expression, typeof(object));
            return Expression.Lambda(castedProperty, param);
        }
    }
}