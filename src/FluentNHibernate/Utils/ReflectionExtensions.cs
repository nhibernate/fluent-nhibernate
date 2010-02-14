using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Utils
{
    public static class ReflectionExtensions
    {
        public static Member ToMember<TMapping, TReturn>(this Expression<Func<TMapping, TReturn>> propertyExpression)
        {
            return ReflectionHelper.GetMember(propertyExpression);
        }
    }
}