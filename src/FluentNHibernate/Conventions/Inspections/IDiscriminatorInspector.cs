using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IDiscriminatorInspector : IInspector
    {
        bool Insert { get; }
        string Column { get; }
        bool Force { get; }
        string Formula { get; }
        int Length { get; }
        bool NotNull { get; }
        TypeReference Type { get; }
    }
}