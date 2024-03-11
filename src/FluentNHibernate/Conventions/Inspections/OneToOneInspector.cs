using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class OneToOneInspector : IOneToOneInspector
{
    private readonly InspectorModelMapper<IOneToOneInspector, OneToOneMapping> propertyMappings = new InspectorModelMapper<IOneToOneInspector, OneToOneMapping>();
    private readonly OneToOneMapping mapping;

    public OneToOneInspector(OneToOneMapping mapping)
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

    public TypeReference Class
    {
        get
        {
            return mapping.Class;
        }
    }

    public bool Constrained
    {
        get { return mapping.Constrained; }
    }

    public Fetch Fetch
    {
        get { return Fetch.FromString(mapping.Fetch); }
    }

    public string ForeignKey
    {
        get { return mapping.ForeignKey; }
    }

    public Laziness LazyLoad
    {
        get { return new Laziness(mapping.Lazy); }
    }

    public string Name
    {
        get { return mapping.Name; }
    }

    public string PropertyRef
    {
        get { return mapping.PropertyRef; }
    }
}
