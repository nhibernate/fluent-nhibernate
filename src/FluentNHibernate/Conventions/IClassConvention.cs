using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for a single class mapping. Implement this interface to apply
    /// changes to class mappings.
    /// </summary>
    public interface IClassConvention : IConvention<IClassInspector, IClassAlteration>
    {}
}