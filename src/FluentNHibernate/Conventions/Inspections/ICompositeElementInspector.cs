using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICompositeElementInspector : IInspector
    {
        TypeReference Class { get; }
    }
}