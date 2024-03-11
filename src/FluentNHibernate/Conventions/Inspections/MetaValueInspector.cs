using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class MetaValueInspector : IMetaValueInspector
{
    private readonly InspectorModelMapper<IMetaValueInspector, MetaValueMapping> propertyMappings = new InspectorModelMapper<IMetaValueInspector, MetaValueMapping>();
    private readonly MetaValueMapping mapping;

    public MetaValueInspector(MetaValueMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public TypeReference Class => mapping.Class;

    public string Value => mapping.Value;
}
