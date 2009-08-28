using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.Utils
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo ToPropertyInfo<TMapping, TReturn>(this Expression<Func<TMapping, TReturn>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }
    }
}