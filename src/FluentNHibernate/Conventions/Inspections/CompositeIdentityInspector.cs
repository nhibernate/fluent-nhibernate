using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class CompositeIdentityInspector : ICompositeIdentityInspector
{
    private readonly InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping> mappedProperties = new InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping>();
    private readonly CompositeIdMapping mapping;

    public CompositeIdentityInspector(CompositeIdMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(mapping.Access);

    public TypeReference Class => mapping.Class;

    public IEnumerable<IKeyManyToOneInspector> KeyManyToOnes
    {
        get
        {
            return mapping.Keys
                .Where(x => x is KeyManyToOneMapping)
                .Select(x => new KeyManyToOneInspector((KeyManyToOneMapping)x));
        }
    }

    public IEnumerable<IKeyPropertyInspector> KeyProperties
    {
        get
        {
            return mapping.Keys
                .Where(x => x is KeyPropertyMapping)
                .Select(x => new KeyPropertyInspector((KeyPropertyMapping)x));
        }
    }

    public bool Mapped => mapping.Mapped;

    public string Name => mapping.Name;

    public string UnsavedValue => mapping.UnsavedValue;
}
