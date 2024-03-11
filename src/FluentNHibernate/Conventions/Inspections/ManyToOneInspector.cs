using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class ManyToOneInspector : IManyToOneInspector
{
    private readonly InspectorModelMapper<IManyToOneInspector, ManyToOneMapping> propertyMappings = new InspectorModelMapper<IManyToOneInspector, ManyToOneMapping>();
    private readonly ManyToOneMapping mapping;

    public ManyToOneInspector(ManyToOneMapping mapping)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public Access Access => Access.FromString(mapping.Access);

    public NotFound NotFound => NotFound.FromString(mapping.NotFound);

    public string PropertyRef => mapping.PropertyRef;

    public bool Update => mapping.Update;

    public bool Nullable
    {
        get
        {
            if (!mapping.Columns.Any())
                return false;

            return !mapping.Columns.First().NotNull;
        }
    }
    public bool OptimisticLock => mapping.OptimisticLock;

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        var mappedProperty = propertyMappings.Get(property);

        return mapping.Columns.Any(x => x.IsSpecified(mappedProperty)) ||
               mapping.IsSpecified(mappedProperty);
    }

    public Member Property => mapping.Member;

    public string Name => mapping.Name;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x));
        }
    }

    public Cascade Cascade => Cascade.FromString(mapping.Cascade);

    public string Formula => mapping.Formula;

    public TypeReference Class => mapping.Class;

    public Fetch Fetch => Fetch.FromString(mapping.Fetch);

    public string ForeignKey => mapping.ForeignKey;

    public bool Insert => mapping.Insert;

    public Laziness LazyLoad => new(mapping.Lazy);
}
