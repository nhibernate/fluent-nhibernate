using System;

namespace FluentNHibernate.Mapping
{
    public interface IRelationship : IMappingPart
    {
        Type EntityType { get; }
    }
}