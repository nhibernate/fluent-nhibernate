using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class ParentInspector : IParentInspector
{
    private readonly InspectorModelMapper<IPropertyInspector, ParentMapping> mappedProperties = new InspectorModelMapper<IPropertyInspector, ParentMapping>();
    private readonly ParentMapping mapping;

    public ParentInspector(ParentMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public string Name => mapping.Name;

    public Access Access
    {
        get
        {
            if (mapping.Access is not null)
                return Access.FromString(mapping.Access);
             
            return null;
        }
    }
}
