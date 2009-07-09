using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToManyInstance : IManyToManyInspector, IRelationshipInstance
    {
        void ColumnName(string columnName);
        new IDefaultableEnumerable<IColumnInstance> Columns { get; }
    }
}