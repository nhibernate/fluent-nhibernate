using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Subclass convention, implement this interface to alter subclass mappings.
    /// </summary>
    public interface ISubclassConvention : IConvention<ISubclassInspector, ISubclassInstance>
    { }
}