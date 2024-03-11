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

    public Access Access
    {
        get { return Access.FromString(mapping.Access); }
    }

    public Cascade Cascade
    {
        get { return Cascade.FromString(mapping.Cascade); }
    }

    public IEnumerable<IColumnInspector> IdentifierColumns
    {
        get
        {
            return mapping.IdentifierColumns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public string IdType
    {
        get { return mapping.IdType; }
    }

    public bool Insert
    {
        get { return mapping.Insert; }
    }

    public TypeReference MetaType
    {
        get { return mapping.MetaType; }
    }

    public IEnumerable<IMetaValueInspector> MetaValues
    {
        get
        {
            return mapping.MetaValues
                .Select(x => new MetaValueInspector(x))
                .Cast<IMetaValueInspector>();
        }
    }

    public string Name
    {
        get { return mapping.Name; }
    }

    public IEnumerable<IColumnInspector> TypeColumns
    {
        get
        {
            return mapping.TypeColumns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public bool Update
    {
        get { return mapping.Update; }
    }

    public bool LazyLoad
    {
        get { return mapping.Lazy; }
    }

    public bool OptimisticLock
    {
        get { return mapping.OptimisticLock; }
    }
}
