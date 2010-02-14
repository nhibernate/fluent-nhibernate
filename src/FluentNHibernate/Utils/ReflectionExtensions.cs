using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Utils
{
    public static class ReflectionExtensions
    {
        public static Member ToMember<TMapping, TReturn>(this Expression<Func<TMapping, TReturn>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression).ToMember();
        }
    }
}