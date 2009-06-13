using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IRelationshipInspector :IInspector
    {
        IDefaultableEnumerable<ColumnMapping> Columns { get; }
    }
}