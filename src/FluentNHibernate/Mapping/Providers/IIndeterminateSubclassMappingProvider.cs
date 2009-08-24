using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIndeterminateSubclassMappingProvider
    {
        ISubclassMapping GetSubclassMapping(ISubclassMapping mapping);
        Type EntityType { get; }
    }
}