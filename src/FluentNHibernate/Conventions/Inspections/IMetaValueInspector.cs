using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IMetaValueInspector : IInspector
    {
        TypeReference Class { get; }
        string Value { get; }
    }
}