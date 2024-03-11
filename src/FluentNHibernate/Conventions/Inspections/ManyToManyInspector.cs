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

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Class.Name; }
    }

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

    public Type ChildType
    {
        get { return mapping.ChildType; }
    }

    public TypeReference Class
    {
        get { return mapping.Class; }
    }

    public Fetch Fetch
    {
        get { return Fetch.FromString(mapping.Fetch); }
    }

    public string ForeignKey
    {
        get { return mapping.ForeignKey; }
    }

    public bool LazyLoad
    {
        get { return mapping.Lazy; }
    }

    public NotFound NotFound
    {
        get { return NotFound.FromString(mapping.NotFound); }
    }

    public Type ParentType
    {
        get { return mapping.ParentType; }
    }

    public string Where
    {
        get { return mapping.Where; }
    }

    public string OrderBy
    {
        get { return mapping.OrderBy; }
    }
}
