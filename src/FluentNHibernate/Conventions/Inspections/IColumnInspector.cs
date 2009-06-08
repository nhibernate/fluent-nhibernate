namespace FluentNHibernate.Conventions.Inspections
{
    public interface IColumnInspector : IInspector
    {
        string Name { get; }
    }
}