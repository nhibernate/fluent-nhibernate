using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IInspector
    {
        Type EntityType { get; }

        bool IsSet(PropertyInfo property);
    }
}