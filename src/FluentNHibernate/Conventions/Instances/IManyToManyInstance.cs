using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToManyInstance : IManyToManyInspector, IRelationshipInstance
    {
        void Column(string columnName);
        new IDefaultableEnumerable<IColumnInstance> Columns { get; }
    }
}