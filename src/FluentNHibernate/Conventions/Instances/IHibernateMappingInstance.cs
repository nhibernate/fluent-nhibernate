using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IHibernateMappingInstance : IHibernateMappingInspector
    {
        void Catalog(string catalog);
        void Schema(string schema);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IHibernateMappingInstance Not { get; }
        void DefaultLazy();
        void AutoImport();
        new ICascadeInstance DefaultCascade { get; }
        new IAccessInstance DefaultAccess { get; }
    }
}