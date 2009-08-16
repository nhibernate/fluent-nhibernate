using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToManyInspector : IRelationshipInspector
    {
        Type ChildType { get; }
        TypeReference Class { get; }
        NotFound NotFound { get; }
    }
}