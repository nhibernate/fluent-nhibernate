using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

/// <summary>
/// Join convention, implement this interface to alter join mappings.
/// </summary>
public interface IJoinConvention : IConvention<IJoinInspector, IJoinInstance>
{ }
