using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class DiscriminatorInspector : ColumnBasedInspector, IDiscriminatorInspector
{
    private readonly InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping> propertyMappings = new InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping>();
    private readonly DiscriminatorMapping mapping;

    public DiscriminatorInspector(DiscriminatorMapping mapping)
        : base(mapping.Columns)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public bool Insert
    {
        get { return mapping.Insert; }
    }

    public bool Force
    {
        get { return mapping.Force; }
    }

    public string Formula
    {
        get { return mapping.Formula; }
    }

    public TypeReference Type
    {
        get { return mapping.Type; }
    }

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Type.Name; }
    }

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
