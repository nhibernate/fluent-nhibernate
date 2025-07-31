using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class PropertyInspector : ColumnBasedInspector, IPropertyInspector
{
    readonly InspectorModelMapper<IPropertyInspector, PropertyMapping> propertyMappings = new InspectorModelMapper<IPropertyInspector, PropertyMapping>();
    readonly PropertyMapping mapping;

    public PropertyInspector(PropertyMapping mapping)
        : base(mapping.Columns)
    {
        this.mapping = mapping;

        propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
        propertyMappings.Map(x => x.Nullable, "NotNull");
    }

    public bool Insert => mapping.Insert;

    public bool Update => mapping.Update;

    public string Formula => mapping.Formula;

    public TypeReference Type => mapping.Type;

    public string Name => mapping.Name;

    public bool OptimisticLock => mapping.OptimisticLock;

    public Generated Generated => Generated.FromString(mapping.Generated);

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

    public bool LazyLoad => mapping.Lazy;

    public Access Access
    {
        get
        {
            if (mapping.Access is not null)
                return Access.FromString(mapping.Access);

            return Access.Unset;
        }
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool ReadOnly => mapping.Insert && mapping.Update;

    public Member Property => mapping.Member;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
