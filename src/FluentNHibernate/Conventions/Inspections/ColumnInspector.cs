using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class ColumnInspector : IColumnInspector
{
    private readonly ColumnMapping mapping;
    private readonly InspectorModelMapper<IColumnInspector, ColumnMapping> propertyMappings = new InspectorModelMapper<IColumnInspector, ColumnMapping>();

    public ColumnInspector(Type containingEntityType, ColumnMapping mapping)
    {
        EntityType = containingEntityType;
        this.mapping = mapping;
    }

    public Type EntityType { get; private set; }

    public string Name
    {
        get { return mapping.Name; }
    }

    public string Check
    {
        get { return mapping.Check; }
    }

    public string Index
    {
        get { return mapping.Index; }
    }

    public int Length
    {
        get { return mapping.Length; }
    }

    public bool NotNull
    {
        get { return mapping.NotNull; }
    }

    public string SqlType
    {
        get { return mapping.SqlType; }
    }

    public bool Unique
    {
        get { return mapping.Unique; }
    }

    public string UniqueKey
    {
        get { return mapping.UniqueKey; }
    }

    public int Precision
    {
        get { return mapping.Precision; }
    }

    public int Scale
    {
        get { return mapping.Scale; }
    }

    public string Default
    {
        get { return mapping.Default; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Name; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
