using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for dynamic components. Implement this member to apply changes
    /// to dynamic components.
    /// </summary>
    public interface IDynamicComponentConvention : IConvention<IDynamicComponentInspector, IDynamicComponentInstance>
    { }
}