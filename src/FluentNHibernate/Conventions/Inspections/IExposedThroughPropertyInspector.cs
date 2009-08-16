using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IExposedThroughPropertyInspector : IInspector
    {
        PropertyInfo Property { get; }
    }
}