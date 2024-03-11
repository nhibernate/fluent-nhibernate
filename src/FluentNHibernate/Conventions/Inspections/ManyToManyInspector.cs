using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;

public class ManyToManyInspector : IManyToManyInspector
{
    private readonly InspectorModelMapper<IManyToManyInspector, ManyToManyMapping> mappedProperties = new InspectorModelMapper<IManyToManyInspector, ManyToManyMapping>();
    private readonly ManyToManyMapping mapping;

    public ManyToManyInspector(ManyToManyMapping mapping)
    {
        this.mapping = mapping;
        mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }

    public Type ChildType => mapping.ChildType;

    public TypeReference Class => mapping.Class;

    public Fetch Fetch => Fetch.FromString(mapping.Fetch);

    public string ForeignKey => mapping.ForeignKey;

    public bool LazyLoad => mapping.Lazy;

    public NotFound NotFound => NotFound.FromString(mapping.NotFound);

    public Type ParentType => mapping.ParentType;

    public string Where => mapping.Where;

    public string OrderBy => mapping.OrderBy;
}
