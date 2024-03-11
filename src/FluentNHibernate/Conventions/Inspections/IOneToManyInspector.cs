using System;

namespace FluentNHibernate.Conventions.Inspections;

public interface IOneToManyInspector : IRelationshipInspector
{
    Type ChildType { get; }
    NotFound NotFound { get; }
}
