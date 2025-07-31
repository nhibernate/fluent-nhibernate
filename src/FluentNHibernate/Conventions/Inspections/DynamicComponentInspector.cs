using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public class DynamicComponentInspector(IComponentMapping mapping)
    : ComponentBaseInspector(mapping), IDynamicComponentInspector
{
    readonly InspectorModelMapper<IDynamicComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IDynamicComponentInspector, ComponentMapping>();
    readonly IComponentMapping mapping = mapping;

    public override bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }
}
