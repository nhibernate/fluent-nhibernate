using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IHibernateMappingInstance : IHibernateMappingInspector
    {
        new void Catalog(string catalog);
        new void Schema(string schema);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IHibernateMappingInstance Not { get; }
        new void DefaultLazy();
        new void AutoImport();
        new ICascadeInstance DefaultCascade { get; }
        new IAccessInstance DefaultAccess { get; }
    }
}