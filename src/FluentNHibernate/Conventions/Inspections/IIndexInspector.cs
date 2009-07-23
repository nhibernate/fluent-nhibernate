using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIndexInspector : IIndexInspectorBase
    {
        TypeReference Type { get; }
    }
}
