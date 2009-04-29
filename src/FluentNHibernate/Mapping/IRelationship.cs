using System;

namespace FluentNHibernate.Mapping
{
    public interface IRelationship : IHasAttributes, IMappingPart
    {
        Type EntityType { get; }
        IAccessStrategyBuilder Access { get; }
    }
}