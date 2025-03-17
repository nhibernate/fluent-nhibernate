using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

/// <summary>
/// Joined subclass convention, implement this interface to alter joined-subclass mappings.
/// </summary>
public interface IJoinedSubclassConvention : IConvention<IJoinedSubclassInspector, IJoinedSubclassInstance>
{ }
