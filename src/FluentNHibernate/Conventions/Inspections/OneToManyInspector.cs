using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;

public class OneToManyInspector : IOneToManyInspector
{
    private readonly InspectorModelMapper<IOneToManyInspector, OneToManyMapping> mappedProperties = new InspectorModelMapper<IOneToManyInspector, OneToManyMapping>();
    private readonly OneToManyMapping mapping;

    public OneToManyInspector(OneToManyMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Class.Name; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public Type ChildType
    {
        get { return mapping.ChildType; }
    }

    public TypeReference Class
    {
        get { return mapping.Class; }
    }

    public NotFound NotFound
    {
        get { return NotFound.FromString(mapping.NotFound); }
    }

}
