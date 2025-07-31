using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class MetaValueInspector(MetaValueMapping mapping) : IMetaValueInspector
{
    readonly InspectorModelMapper<IMetaValueInspector, MetaValueMapping> propertyMappings = new InspectorModelMapper<IMetaValueInspector, MetaValueMapping>();

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public TypeReference Class => mapping.Class;

    public string Value => mapping.Value;
}
