using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IRelationshipInspector : IInspector
    {
        TypeReference Class { get; }
    }
}