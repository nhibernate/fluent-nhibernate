using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIndexManyToManyInspector : IIndexInspectorBase
    {
        TypeReference Class { get; }
        string ForeignKey { get; }
    }
}
