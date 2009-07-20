using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IVersionInspector : IInspector
    {
        string Name { get; }
        Access Access { get; }
        string Column { get; }
        Generated Generated { get; }
        string UnsavedValue { get; }
        TypeReference Type { get; }
    }
}