using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class KeyInspector : IKeyInspector
{
    private readonly InspectorModelMapper<IKeyInspector, KeyMapping> propertyMappings = new InspectorModelMapper<IKeyInspector, KeyMapping>();
    private readonly KeyMapping mapping;

    public KeyInspector(KeyMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return ""; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public string ForeignKey
    {
        get { return mapping.ForeignKey; }
    }

    public OnDelete OnDelete
    {
        get { return OnDelete.FromString(mapping.OnDelete); }
    }

    public string PropertyRef
    {
        get { return mapping.PropertyRef; }
    }
}
