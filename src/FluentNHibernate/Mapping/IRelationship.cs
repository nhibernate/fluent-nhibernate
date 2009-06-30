using System;

namespace FluentNHibernate.Mapping
{
    public interface IRelationship
    {
        Type EntityType { get; }
        IAccessStrategyBuilder Access { get; }
    }
}