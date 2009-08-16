using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for a single class mapping. Implement this interface to apply
    /// changes to class mappings.
    /// </summary>
    public interface IClassConvention : IConvention<IClassInspector, IClassInstance>
    {}
}