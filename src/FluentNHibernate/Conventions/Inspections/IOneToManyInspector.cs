using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToManyInspector : IRelationshipInspector
    {
        Type ChildType { get; }
        NotFound NotFound { get; }
    }
}