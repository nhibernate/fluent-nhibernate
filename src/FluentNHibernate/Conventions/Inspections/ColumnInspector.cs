using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class ColumnInspector(Type containingEntityType, ColumnMapping mapping) : IColumnInspector
{
    readonly InspectorModelMapper<IColumnInspector, ColumnMapping> propertyMappings = new InspectorModelMapper<IColumnInspector, ColumnMapping>();

    public Type EntityType { get; } = containingEntityType;

    public string Name => mapping.Name;

    public string Check => mapping.Check;

    public string Index => mapping.Index;

    public int Length => mapping.Length;

    public bool NotNull => mapping.NotNull;

    public string SqlType => mapping.SqlType;

    public bool Unique => mapping.Unique;

    public string UniqueKey => mapping.UniqueKey;

    public int Precision => mapping.Precision;

    public int Scale => mapping.Scale;

    public string Default => mapping.Default;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
