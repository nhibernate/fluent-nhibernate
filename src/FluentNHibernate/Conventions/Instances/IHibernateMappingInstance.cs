using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IHibernateMappingInstance : IHibernateMappingInspector
    {
        void Catalog(string catalog);
        void Schema(string schema);
        IHibernateMappingInstance Not { get; }
        void DefaultLazy();
        new ICascadeInstance DefaultCascade { get; }
        new IAccessInstance DefaultAccess { get; }
    }
}