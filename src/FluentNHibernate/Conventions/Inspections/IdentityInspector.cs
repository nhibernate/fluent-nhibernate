using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class IdentityInspector : ColumnBasedInspector, IIdentityInspector
{
    private readonly InspectorModelMapper<IIdentityInspector, IdMapping> propertyMappings = new InspectorModelMapper<IIdentityInspector, IdMapping>();
    private readonly IdMapping mapping;

    public IdentityInspector(IdMapping mapping)
        : base(mapping.Columns)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Name; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public Member Property
    {
        get { return mapping.Member; }
    }

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(EntityType, x))
                .Cast<IColumnInspector>();
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

    public string UnsavedValue
    {
        get { return mapping.UnsavedValue; }
    }

    public string Name
    {
        get { return mapping.Name; }
    }

    public Access Access
    {
        get { return Access.FromString(mapping.Access); }
    }

    public TypeReference Type
    {
        get { return mapping.Type; }
    }
}
