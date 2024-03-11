using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class KeyPropertyInspector : IKeyPropertyInspector
{
    private readonly InspectorModelMapper<IKeyPropertyInspector, KeyPropertyMapping> mappedProperties = new InspectorModelMapper<IKeyPropertyInspector, KeyPropertyMapping>();
    private readonly KeyPropertyMapping mapping;

    public KeyPropertyInspector(KeyPropertyMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(mapping.Access);

    public string Name => mapping.Name;

    public TypeReference Type => mapping.Type;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x));
        }
    }
    public int Length => mapping.Length;
}
