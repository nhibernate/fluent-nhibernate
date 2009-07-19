using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyInspector : IRelationshipInspector
    {
        IDefaultableEnumerable<IColumnInspector> Columns { get; }
        Type ChildType { get; }
        TypeReference Class { get; }
        string Fetch { get; }
        string ForeignKey { get; }
        Laziness LazyLoad { get; }
        string NotFound { get; }
        string OuterJoin { get; }
        Type ParentType { get; }
        string Where { get; }
    }
}