using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIndeterminateSubclassMappingProvider
    {
        SubclassMapping GetSubclassMapping(SubclassType type);
        Type EntityType { get; }
        Type Extends { get; }
    }
}