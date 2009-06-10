using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Reference convention, implement this interface to apply changes to Reference/many-to-one
    /// relationships.
    /// </summary>
    public interface IReferenceConvention : IConvention<IManyToOneInspector, IManyToOneAlteration>
    { }
}