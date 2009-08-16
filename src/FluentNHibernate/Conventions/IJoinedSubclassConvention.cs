using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Joined subclass convention, implement this interface to alter joined-subclass mappings.
    /// </summary>
    public interface IJoinedSubclassConvention : IConvention<IJoinedSubclassInspector, IJoinedSubclassInstance>
    { }
}