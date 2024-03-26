using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;

public class IndexManyToManyInspector(IndexManyToManyMapping mapping) : IIndexManyToManyInspector
{
    readonly InspectorModelMapper<IIndexManyToManyInspector, IndexManyToManyMapping> mappedProperties = new InspectorModelMapper<IIndexManyToManyInspector, IndexManyToManyMapping>();

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Class.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }
        
    public TypeReference Class => mapping.Class;

    public string ForeignKey => mapping.ForeignKey;

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x));
        }
    }
}
