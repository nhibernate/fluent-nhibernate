using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class IdentityInspector : ColumnBasedInspector, IIdentityInspector
{
    readonly InspectorModelMapper<IIdentityInspector, IdMapping> propertyMappings = new InspectorModelMapper<IIdentityInspector, IdMapping>();
    readonly IdMapping mapping;

    public IdentityInspector(IdMapping mapping)
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

    public Member Property => mapping.Member;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(EntityType, x));
        }
    }

    public IGeneratorInspector Generator
    {
        get
        {
            if (mapping.Generator is null)
                return new GeneratorInspector(new GeneratorMapping());

            return new GeneratorInspector(mapping.Generator);
        }
    }

    public string UnsavedValue => mapping.UnsavedValue;

    public string Name => mapping.Name;

    public Access Access => Access.FromString(mapping.Access);

    public TypeReference Type => mapping.Type;
}
