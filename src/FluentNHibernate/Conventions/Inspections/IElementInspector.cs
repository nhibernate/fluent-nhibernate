using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IElementInspector : IInspector
    {
        TypeReference Type { get; }
    }
}