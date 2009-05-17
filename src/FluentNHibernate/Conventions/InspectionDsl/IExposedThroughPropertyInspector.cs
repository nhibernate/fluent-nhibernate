using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.InspectionDsl
{
    public interface IExposedThroughPropertyInspector : IInspector
    {
        Type PropertyType { get; }
        PropertyInfo Property { get; }
    }
}