using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions;

/// <summary>
/// Reference convention, implement this interface to apply changes to Reference/many-to-one
/// relationships.
/// </summary>
public interface IReferenceConvention : IConvention<IManyToOneInspector, IManyToOneInstance>
{ }
