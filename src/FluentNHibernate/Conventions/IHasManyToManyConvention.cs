using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// HasManyToMany convention, used on many-to-many relationships.
    /// </summary>
    public interface IHasManyToManyConvention : IConvention<IManyToManyInspector, IManyToManyAlteration>
    { }
}