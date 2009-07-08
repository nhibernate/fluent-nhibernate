using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IRelationshipInstance : IRelationshipInspector, IRelationshipAlteration
    {
        new IDefaultableEnumerable<IColumnInstance> Columns { get; }
    }
}