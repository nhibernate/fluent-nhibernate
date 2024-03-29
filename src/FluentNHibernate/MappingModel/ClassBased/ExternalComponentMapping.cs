using System;

namespace FluentNHibernate.MappingModel.ClassBased;

/// <summary>
/// A component that is declared external to a class mapping.
/// </summary>
[Serializable]
public class ExternalComponentMapping(ComponentType componentType, AttributeStore underlyingStore, Member member)
    : ComponentMapping(componentType, underlyingStore, member)
{
    public ExternalComponentMapping(ComponentType componentType): this(componentType, new AttributeStore(), null)
    {}
}
