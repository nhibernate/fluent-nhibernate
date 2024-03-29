using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class DiscriminatorInspector : ColumnBasedInspector, IDiscriminatorInspector
{
    readonly InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping> propertyMappings = new InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping>();
    readonly DiscriminatorMapping mapping;

    public DiscriminatorInspector(DiscriminatorMapping mapping)
        : base(mapping.Columns)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public bool Insert => mapping.Insert;

    public bool Force => mapping.Force;

    public string Formula => mapping.Formula;

    public TypeReference Type => mapping.Type;

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Type.Name;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>()
                .ToList();
        }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
