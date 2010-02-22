using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Utils
{
    public interface Accessor
    {
        string FieldName { get; }

        Type PropertyType { get; }
        Member InnerMember { get; }
        void SetValue(object target, object propertyValue);
        object GetValue(object target);

        Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression);

        string Name { get; }
    }
}