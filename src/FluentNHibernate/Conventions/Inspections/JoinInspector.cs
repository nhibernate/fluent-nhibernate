using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public class JoinInspector : IJoinInspector
{
    private readonly InspectorModelMapper<IJoinInspector, JoinMapping> propertyMappings = new InspectorModelMapper<IJoinInspector, JoinMapping>();
    private readonly JoinMapping mapping;

    public JoinInspector(JoinMapping mapping)
    {
        this.mapping = mapping;
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.TableName;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public IEnumerable<IAnyInspector> Anys
    {
        get
        {
            return mapping.Anys
                .Select(x => new AnyInspector(x));
        }
    }

    public Fetch Fetch => Fetch.FromString(mapping.Fetch);

    public bool Inverse => mapping.Inverse;

    public IKeyInspector Key
    {
        get
        {
            if (mapping.Key is null)
                return new KeyInspector(new KeyMapping());

            return new KeyInspector(mapping.Key);
        }
    }

    public bool Optional => mapping.Optional;

    public IEnumerable<IPropertyInspector> Properties
    {
        get
        {
            return mapping.Properties
                .Select(x => new PropertyInspector(x));
        }
    }

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x));
        }
    }


    public IEnumerable<ICollectionInspector> Collections
    {
        get
        {
            return mapping.Collections
                .Select(x => new CollectionInspector(x));
        }
    }

    public string Schema => mapping.Schema;

    public string TableName => mapping.TableName;

    public string Catalog => mapping.Catalog;

    public string Subselect => mapping.Subselect;
}
