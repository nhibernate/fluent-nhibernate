using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CollectionInspector : ICollectionInspector
    {
        private readonly InspectorModelMapper<ICollectionInspector, ICollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, ICollectionMapping>();
        private readonly ICollectionMapping mapping;

        public CollectionInspector(ICollectionMapping mapping)
        {
            this.mapping = mapping;
            propertyMappings.AutoMap();
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

        /// <summary>
        /// Represents a string identifier for the model instance, used in conventions for a lazy
        /// shortcut.
        /// 
        /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
        /// this allows the user to find any columns with the matching name.
        /// </summary>
        public bool IsSet(PropertyInfo property)
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
            get { return mapping.MemberInfo is MethodInfo; }
        }

        public MemberInfo Member
        {
            get { return mapping.MemberInfo; }
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

        public bool LazyLoad
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
    }
}