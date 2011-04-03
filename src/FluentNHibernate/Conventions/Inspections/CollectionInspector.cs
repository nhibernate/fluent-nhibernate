using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
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

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        Collection ICollectionInspector.Collection
        {
            get { return mapping.Collection; }
        }

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
                if (mapping.Key == null)
                    return new KeyInspector(new KeyMapping());

                return new KeyInspector(mapping.Key);
            }
        }

        public string TableName
        {
            get { return mapping.TableName; }
        }

        public bool IsMethodAccess
        {
            get { return mapping.Member.IsMethod; }
        }

        public MemberInfo Member
        {
            get { return mapping.Member.MemberInfo; }
        }

        public IRelationshipInspector Relationship
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyInspector((ManyToManyMapping)mapping.Relationship);

                return new OneToManyInspector((OneToManyMapping)mapping.Relationship);
            }
        }

        public Cascade Cascade
        {
            get { return Cascade.FromString(mapping.Cascade); }
        }

        public Fetch Fetch
        {
            get { return Fetch.FromString(mapping.Fetch); }
        }

        public OptimisticLock OptimisticLock
        {
            get { return OptimisticLock.FromString(mapping.OptimisticLock); }
        }

        public bool Generic
        {
            get { return mapping.Generic; }
        }

        public bool Inverse
        {
            get { return mapping.Inverse; }
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public int BatchSize
        {
            get { return mapping.BatchSize; }
        }

        public ICacheInspector Cache
        {
            get
            {
                if (mapping.Cache == null)
                    return new CacheInspector(new CacheMapping());

                return new CacheInspector(mapping.Cache);
            }
        }

        public string Check
        {
            get { return mapping.Check; }
        }

        public Type ChildType
        {
            get { return mapping.ChildType; }
        }

        public TypeReference CollectionType
        {
            get { return mapping.CollectionType; }
        }

        public ICompositeElementInspector CompositeElement
        {
            get
            {
                if (mapping.CompositeElement == null)
                    return new CompositeElementInspector(new CompositeElementMapping());

                return new CompositeElementInspector(mapping.CompositeElement);
            }
        }

        public IElementInspector Element
        {
            get
            {
                if (mapping.Element == null)
                    return new ElementInspector(new ElementMapping());

                return new ElementInspector(mapping.Element);
            }
        }

        public Lazy LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public TypeReference Persister
        {
            get { return mapping.Persister; }
        }

        public string Schema
        {
            get { return mapping.Schema; }
        }

        public string Where
        {
            get { return mapping.Where; }
        }

        public string OrderBy
        {
            get { return mapping.OrderBy; }
        }

        public string Sort
        {
            get { return mapping.Sort; }
        }

        public IIndexInspectorBase Index
        {
            get
            {
                if (mapping.Index == null)
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
}