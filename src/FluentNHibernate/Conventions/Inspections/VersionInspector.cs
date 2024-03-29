using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class VersionInspector : ColumnBasedInspector, IVersionInspector
{
    readonly InspectorModelMapper<IVersionInspector, VersionMapping> propertyMappings = new InspectorModelMapper<IVersionInspector, VersionMapping>();
    readonly VersionMapping mapping;

    public VersionInspector(VersionMapping mapping)
        : base(mapping.Columns)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public string Name => mapping.Name;

    public Access Access => Access.FromString(mapping.Access);

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

    public Generated Generated => Generated.FromString(mapping.Generated);

    public string UnsavedValue => mapping.UnsavedValue;

    public TypeReference Type => mapping.Type;
}
