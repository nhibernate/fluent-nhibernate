using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for a component mapping. Implement this interface to
    /// apply changes to components.
    /// </summary>
    public interface IComponentConvention : IConvention<IComponentInspector, IComponentInstance>
    { }
}