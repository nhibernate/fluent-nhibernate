using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Property convention, implement this interface to apply changes to
    /// property mappings.
    /// </summary>
    public interface IPropertyConvention : IConvention<IPropertyInspector, IPropertyAlteration>
    {}
}