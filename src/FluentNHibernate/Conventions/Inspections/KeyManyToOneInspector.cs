using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class KeyManyToOneInspector : IKeyManyToOneInspector
{
    private readonly InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping> mappedProperties = new InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping>();
    private readonly KeyManyToOneMapping mapping;

    public KeyManyToOneInspector(KeyManyToOneMapping mapping)
    {
        this.mapping = mapping;
        mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(mapping.Access);

    public TypeReference Class => mapping.Class;

    public string ForeignKey => mapping.ForeignKey;

    public bool LazyLoad => mapping.Lazy;

    public string Name => mapping.Name;

    public NotFound NotFound => NotFound.FromString(mapping.NotFound);

    public IEnumerable<IColumnInspector> Columns
    {
        get
        {
            return mapping.Columns
                .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                .Cast<IColumnInspector>();
        }
    }
}
