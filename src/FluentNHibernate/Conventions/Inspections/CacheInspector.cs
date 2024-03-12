using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class CacheInspector(CacheMapping mapping) : ICacheInspector
{
    private readonly InspectorModelMapper<ICacheInspector, CacheMapping> propertyMappings = new InspectorModelMapper<ICacheInspector, CacheMapping>();

    public string Usage => mapping.Usage;

    public string Region => mapping.Region;

    public Include Include => Include.FromString(mapping.Include);

    public Type EntityType => mapping.ContainedEntityType;

    public string StringIdentifierForModel => mapping.Usage;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
