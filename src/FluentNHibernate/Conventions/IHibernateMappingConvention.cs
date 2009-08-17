using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for the hibernate-mapping container for a class, this can be used to
    /// set some class-wide settings such as lazy-load and access strategies.
    /// </summary>
    public interface IHibernateMappingConvention : IConvention<IHibernateMappingInspector, IHibernateMappingInstance>
    {}
}