using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class OneToOneInspector : IOneToOneInspector
{
    readonly InspectorModelMapper<IOneToOneInspector, OneToOneMapping> propertyMappings = new InspectorModelMapper<IOneToOneInspector, OneToOneMapping>();
    readonly OneToOneMapping mapping;

    public OneToOneInspector(OneToOneMapping mapping)
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

    public TypeReference Class => mapping.Class;

    public bool Constrained => mapping.Constrained;

    public Fetch Fetch => Fetch.FromString(mapping.Fetch);

    public string ForeignKey => mapping.ForeignKey;

    public Laziness LazyLoad => new(mapping.Lazy);

    public string Name => mapping.Name;

    public string PropertyRef => mapping.PropertyRef;
}
