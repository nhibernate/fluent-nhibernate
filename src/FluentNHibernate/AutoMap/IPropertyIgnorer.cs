using System;
using System.Reflection;

namespace FluentNHibernate.AutoMap
{
    public interface IPropertyIgnorer
    {
        IPropertyIgnorer IgnoreProperty(string name);
        IPropertyIgnorer IgnoreProperties(string first, string second, params string[] others);
        IPropertyIgnorer IgnoreProperties(Func<PropertyInfo, bool> predicate);
    }
}