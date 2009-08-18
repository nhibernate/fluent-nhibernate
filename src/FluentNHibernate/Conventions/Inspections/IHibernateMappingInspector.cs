namespace FluentNHibernate.Conventions.Inspections
{
    public interface IHibernateMappingInspector : IInspector
    {
        string Catalog { get; }
        Access DefaultAccess { get; }
        Cascade DefaultCascade { get; }
        bool DefaultLazy { get; }
        bool AutoImport { get; }
        string Schema { get; }
    }
}