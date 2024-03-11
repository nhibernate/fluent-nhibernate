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

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.TableName; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }

    public IEnumerable<IAnyInspector> Anys
    {
        get
        {
            return mapping.Anys
                .Select(x => new AnyInspector(x))
                .Cast<IAnyInspector>();
        }
    }

    public Fetch Fetch
    {
        get { return Fetch.FromString(mapping.Fetch); }
    }

    public bool Inverse
    {
        get { return mapping.Inverse; }
    }

    public IKeyInspector Key
    {
        get
        {
            if (mapping.Key is null)
                return new KeyInspector(new KeyMapping());

            return new KeyInspector(mapping.Key);
        }
    }

    public bool Optional
    {
        get { return mapping.Optional; }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
        get
        {
            return mapping.Properties
                .Select(x => new PropertyInspector(x))
                .Cast<IPropertyInspector>();
        }
    }

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x))
                .Cast<IManyToOneInspector>();
        }
    }


    public IEnumerable<ICollectionInspector> Collections
    {
        get
        {
            return mapping.Collections
                .Select(x => new CollectionInspector(x))
                .Cast<ICollectionInspector>();
        }
    }

    public string Schema
    {
        get { return mapping.Schema; }
    }

    public string TableName
    {
        get { return mapping.TableName; }
    }
        
    public string Catalog
    {
        get { return mapping.Catalog; }
    }

    public string Subselect
    {
        get { return mapping.Subselect; }
    }
}
