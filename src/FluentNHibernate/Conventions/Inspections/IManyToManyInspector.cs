using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyInspector : IRelationshipInspector
    {
        IDefaultableEnumerable<IColumnInspector> Columns { get; }
    }
}