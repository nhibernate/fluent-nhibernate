using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Version convention, implement this interface to apply changes to vesion mappings.
    /// </summary>
    public interface IVersionConvention : IConvention<IVersionInspector, IVersionInstance>
    { }
}