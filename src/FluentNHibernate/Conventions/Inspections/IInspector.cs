using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IInspector
    {
        Type EntityType { get; }
        string StringIdentifierForModel { get; }

        bool IsSet(PropertyInfo property);
    }
}