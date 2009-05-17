using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IExposedThroughPropertyInspector : IInspector
    {
        Type PropertyType { get; }
        PropertyInfo Property { get; }
    }
}