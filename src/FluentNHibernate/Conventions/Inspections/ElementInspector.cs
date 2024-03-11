using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;

public class ElementInspector : IElementInspector
{
    private readonly InspectorModelMapper<IElementInspector, ElementMapping> mappedProperties = new InspectorModelMapper<IElementInspector, ElementMapping>();
    private readonly ElementMapping mapping;

    public ElementInspector(ElementMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Type.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public TypeReference Type => mapping.Type;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public string Formula => mapping.Formula;

    public int Length
    {
        get { return mapping.Columns.Select(x => x.Length).FirstOrDefault();  }
    }
}
