using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public class DynamicComponentInspector(IComponentMapping mapping)
    : ComponentBaseInspector(mapping), IDynamicComponentInspector
{
    private readonly InspectorModelMapper<IDynamicComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IDynamicComponentInspector, ComponentMapping>();
    private readonly IComponentMapping mapping = mapping;

    public override bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }
}
