using System;

namespace FluentNHibernate.Automapping
{
    public interface IPropertyIgnorer
    {
        IPropertyIgnorer IgnoreProperty(string name);
        IPropertyIgnorer IgnoreProperties(string first, params string[] others);
        IPropertyIgnorer IgnoreProperties(Func<Member, bool> predicate);
    }
}