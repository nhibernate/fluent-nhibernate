using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

/// <summary>
/// Version convention, implement this interface to apply changes to version mappings.
/// </summary>
public interface IVersionConvention : IConvention<IVersionInspector, IVersionInstance>
{ }
