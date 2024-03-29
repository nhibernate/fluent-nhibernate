using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections;
#pragma warning disable 612,618
public class CollectionInspector : ICollectionInspector,
    IArrayInspector, IBagInspector, IListInspector, IMapInspector, ISetInspector
#pragma warning restore 612,618
{
    InspectorModelMapper<ICollectionInspector, CollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, CollectionMapping>();
    CollectionMapping mapping;

    public CollectionInspector(CollectionMapping mapping)
    {
        this.mapping = mapping;
        propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    Collection ICollectionInspector.Collection => mapping.Collection;

    /// <summary>
    /// Represents a string identifier for the model instance, used in conventions for a lazy
    /// shortcut.
    /// 
    /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
    /// this allows the user to find any columns with the matching name.
    /// </summary>
    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
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

    public string TableName => mapping.TableName;

    public bool IsMethodAccess => mapping.Member.IsMethod;

    public MemberInfo Member => mapping.Member.MemberInfo;

    public IRelationshipInspector Relationship
    {
        get
        {
            if (mapping.Relationship is ManyToManyMapping)
                return new ManyToManyInspector((ManyToManyMapping)mapping.Relationship);

            return new OneToManyInspector((OneToManyMapping)mapping.Relationship);
        }
    }

    public Cascade Cascade => Cascade.FromString(mapping.Cascade);

    public Fetch Fetch => Fetch.FromString(mapping.Fetch);

    public bool OptimisticLock => mapping.OptimisticLock;

    public bool Generic => mapping.Generic;

    public bool Inverse => mapping.Inverse;

    public Access Access => Access.FromString(mapping.Access);

    public int BatchSize => mapping.BatchSize;

    public ICacheInspector Cache
    {
        get
        {
            if (mapping.Cache is null)
                return new CacheInspector(new CacheMapping());

            return new CacheInspector(mapping.Cache);
        }
    }

    public string Check => mapping.Check;

    public Type ChildType => mapping.ChildType;

    public TypeReference CollectionType => mapping.CollectionType;

    public ICompositeElementInspector CompositeElement
    {
        get
        {
            if (mapping.CompositeElement is null)
                return new CompositeElementInspector(new CompositeElementMapping());

            return new CompositeElementInspector(mapping.CompositeElement);
        }
    }

    public IElementInspector Element
    {
        get
        {
            if (mapping.Element is null)
                return new ElementInspector(new ElementMapping());

            return new ElementInspector(mapping.Element);
        }
    }

    public Lazy LazyLoad => mapping.Lazy;

    public string Name => mapping.Name;

    public TypeReference Persister => mapping.Persister;

    public string Schema => mapping.Schema;

    public string Where => mapping.Where;

    public string OrderBy => mapping.OrderBy;

    public string Sort => mapping.Sort;

    public IIndexInspectorBase Index
    {
        get
        {
            if (mapping.Index is null)
                return new IndexInspector(new IndexMapping());

            if (mapping.Index is IndexMapping)
                return new IndexInspector(mapping.Index as IndexMapping);

            if (mapping.Index is IndexManyToManyMapping)
                return new IndexManyToManyInspector(mapping.Index as IndexManyToManyMapping);

            throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
        }
    }

    public virtual void ExtraLazyLoad()
    {
        // TODO: Fix this...
        // I'm having trouble understanding the relationship between CollectionInspector, CollectionInstance, 
        // and their derivative types. I'm sure adding this method on here is not the right way to do this, but 
        // I have to fulfill the ICollectionInspector.ExtraLazyLoad() signature or conventions can't use it.
        throw new NotImplementedException();
    }
}
