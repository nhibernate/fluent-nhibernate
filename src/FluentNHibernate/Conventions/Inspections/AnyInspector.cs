using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class AnyInspector : IAnyInspector
{
    private readonly InspectorModelMapper<IAnyInspector, AnyMapping> propertyMappings = new InspectorModelMapper<IAnyInspector, AnyMapping>();
    private readonly AnyMapping mapping;

    public AnyInspector(AnyMapping mapping)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public Access Access => Access.FromString(mapping.Access);

    public Cascade Cascade => Cascade.FromString(mapping.Cascade);

    public IEnumerable<IColumnInspector> IdentifierColumns
    {
        get
        {
            return mapping.IdentifierColumns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public string IdType => mapping.IdType;

    public bool Insert => mapping.Insert;

    public TypeReference MetaType => mapping.MetaType;

    public IEnumerable<IMetaValueInspector> MetaValues
    {
        get
        {
            return mapping.MetaValues
                .Select(x => new MetaValueInspector(x))
                .Cast<IMetaValueInspector>();
        }
    }

    public string Name => mapping.Name;

    public IEnumerable<IColumnInspector> TypeColumns
    {
        get
        {
            return mapping.TypeColumns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public bool Update => mapping.Update;

    public bool LazyLoad => mapping.Lazy;

    public bool OptimisticLock => mapping.OptimisticLock;
}
