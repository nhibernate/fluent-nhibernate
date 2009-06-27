using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Property convention, implement this interface to apply changes to
    /// property mappings.
    /// </summary>
    public interface IPropertyConvention : IConvention<IPropertyInspector, IPropertyAlteration, IPropertyInstance>
    {}
}