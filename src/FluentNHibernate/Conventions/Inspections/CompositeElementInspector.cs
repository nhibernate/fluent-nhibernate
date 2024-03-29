using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;

public class CompositeElementInspector(CompositeElementMapping mapping) : ICompositeElementInspector
{
    readonly InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping> mappedProperties = new InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping>();

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public TypeReference Class => mapping.Class;

    public IParentInspector Parent
    {
        get
        {
            if (mapping.Parent is null)
                return new ParentInspector(new ParentMapping());

            return new ParentInspector(mapping.Parent);
        }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
        get
        {
            return mapping.Properties
                .Select(x => new PropertyInspector(x));
        }
    }

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x));
        }
    }
}
