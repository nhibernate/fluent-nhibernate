using System;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IReferenceComponentMappingProvider : IComponentMappingProvider
    {
        Type Type { get; }
    }
}