using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.InspectionDsl
{
    public interface IInspector
    {
        Type EntityType { get; }

        bool IsSet(PropertyInfo property);
    }
}