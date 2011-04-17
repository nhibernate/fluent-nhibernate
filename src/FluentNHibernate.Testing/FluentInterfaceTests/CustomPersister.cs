using System;
using System.Collections;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.Tuple.Entity;
using NHibernate.Type;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    internal class SecondCustomPersister : CustomPersister
    {}

    internal class CustomPersister : IEntityPersister
    {
        public void PostInstantiate()
        {}

        public bool IsSubclassEntityName(string entityName)
        {
            return false;
        }

        public IType GetPropertyType(string propertyName)
        {
            return null;
        }

        public int[] FindDirty(object[] currentState, object[] previousState, object entity, ISessionImplementor session)
        {
            return new int[] {};
        }

        public int[] FindModified(object[] old, object[] current, object entity, ISessionImplementor session)
        {
            return new int[] {};
        }

        public object[] GetNaturalIdentifierSnapshot(object id, ISessionImplementor session)
        {
            return new object[] {};
        }

        public object Load(object id, object optionalObject, LockMode lockMode, ISessionImplementor session)
        {
            return null;
        }

        public void Lock(object id, object version, object obj, LockMode lockMode, ISessionImplementor session)
        {}

        public void Insert(object id, object[] fields, object obj, ISessionImplementor session)
        {}

        public object Insert(object[] fields, object obj, ISessionImplementor session)
        {
            return null;
        }

        public void Delete(object id, object version, object obj, ISessionImplementor session)
        {}

        public void Update(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, ISessionImplementor session)
        {}

        public object[] GetDatabaseSnapshot(object id, ISessionImplementor session)
        {
            return new object[] {};
        }

        public object GetCurrentVersion(object id, ISessionImplementor session)
        {
            return null;
        }

        public object ForceVersionIncrement(object id, object currentVersion, ISessionImplementor session)
        {
            return null;
        }

        public EntityMode? GuessEntityMode(object obj)
        {
            return null;
        }

        public bool IsInstrumented(EntityMode entityMode)
        {
            return false;
        }

        public void AfterInitialize(object entity, bool lazyPropertiesAreUnfetched, ISessionImplementor session)
        {}

        public void AfterReassociate(object entity, ISessionImplementor session)
        {}

        public object CreateProxy(object id, ISessionImplementor session)
        {
            return null;
        }

        public bool? IsTransient(object obj, ISessionImplementor session)
        {
            return null;
        }

        public object[] GetPropertyValuesToInsert(object obj, IDictionary mergeMap, ISessionImplementor session)
        {
            return new object[] {};
        }

        public void ProcessInsertGeneratedProperties(object id, object entity, object[] state, ISessionImplementor session)
        {}

        public void ProcessUpdateGeneratedProperties(object id, object entity, object[] state, ISessionImplementor session)
        {}

        public Type GetMappedClass(EntityMode entityMode)
        {
            return null;
        }

        public bool ImplementsLifecycle(EntityMode entityMode)
        {
            return false;
        }

        public bool ImplementsValidatable(EntityMode entityMode)
        {
            return false;
        }

        public Type GetConcreteProxyClass(EntityMode entityMode)
        {
            return null;
        }

        public void SetPropertyValues(object obj, object[] values, EntityMode entityMode)
        {}

        public void SetPropertyValue(object obj, int i, object value, EntityMode entityMode)
        {}

        public object[] GetPropertyValues(object obj, EntityMode entityMode)
        {
            return new object[] {};
        }

        public object GetPropertyValue(object obj, int i, EntityMode entityMode)
        {
            return null;
        }

        public object GetPropertyValue(object obj, string name, EntityMode entityMode)
        {
            return null;
        }

        public object GetIdentifier(object obj, EntityMode entityMode)
        {
            return null;
        }

        public void SetIdentifier(object obj, object id, EntityMode entityMode)
        {}

        public object GetVersion(object obj, EntityMode entityMode)
        {
            return null;
        }

        public object Instantiate(object id, EntityMode entityMode)
        {
            return null;
        }

        public bool IsInstance(object entity, EntityMode entityMode)
        {
            return false;
        }

        public bool HasUninitializedLazyProperties(object obj, EntityMode entityMode)
        {
            return false;
        }

        public void ResetIdentifier(object entity, object currentId, object currentVersion, EntityMode entityMode)
        {}

        public IEntityPersister GetSubclassEntityPersister(object instance, ISessionFactoryImplementor factory, EntityMode entityMode)
        {
            return null;
        }

        public bool? IsUnsavedVersion(object version)
        {
            return null;
        }

        public ISessionFactoryImplementor Factory
        {
            get { return null; }
        }
        public string RootEntityName
        {
            get { return null; }
        }
        public string EntityName
        {
            get { return null; }
        }
        public EntityMetamodel EntityMetamodel
        {
            get { return null; }
        }
        public string[] PropertySpaces
        {
            get { return new string[] {}; }
        }
        public string[] QuerySpaces
        {
            get { return new string[] {}; }
        }
        public bool IsMutable
        {
            get { return false; }
        }
        public bool IsInherited
        {
            get { return false; }
        }
        public bool IsIdentifierAssignedByInsert
        {
            get { return false; }
        }
        bool IEntityPersister.IsVersioned
        {
            get { return false; }
        }
        public IVersionType VersionType
        {
            get { return null; }
        }
        public int VersionProperty
        {
            get { return 0; }
        }
        public int[] NaturalIdentifierProperties
        {
            get { return new int[] {}; }
        }
        public IIdentifierGenerator IdentifierGenerator
        {
            get { return null; }
        }
        public IType[] PropertyTypes
        {
            get { return new IType[] {}; }
        }
        public string[] PropertyNames
        {
            get { return new string[] {}; }
        }
        public bool[] PropertyInsertability
        {
            get { return new bool[] {}; }
        }
        public ValueInclusion[] PropertyInsertGenerationInclusions
        {
            get { return new ValueInclusion[] {}; }
        }
        public ValueInclusion[] PropertyUpdateGenerationInclusions
        {
            get { return new ValueInclusion[] {}; }
        }
        public bool[] PropertyCheckability
        {
            get { return new bool[] {}; }
        }
        public bool[] PropertyNullability
        {
            get { return new bool[] {}; }
        }
        public bool[] PropertyVersionability
        {
            get { return new bool[] {}; }
        }
        public bool[] PropertyLaziness
        {
            get { return new bool[] {}; }
        }
        public CascadeStyle[] PropertyCascadeStyles
        {
            get { return new CascadeStyle[] {}; }
        }
        public IType IdentifierType
        {
            get { return null; }
        }
        public string IdentifierPropertyName
        {
            get { return null; }
        }
        public bool IsCacheInvalidationRequired
        {
            get { return false; }
        }
        public bool IsLazyPropertiesCacheable
        {
            get { return false; }
        }
        public ICacheConcurrencyStrategy Cache
        {
            get { return null; }
        }
        public ICacheEntryStructure CacheEntryStructure
        {
            get { return null; }
        }
        public IClassMetadata ClassMetadata
        {
            get { return null; }
        }
        public bool IsBatchLoadable
        {
            get { return false; }
        }
        public bool IsSelectBeforeUpdateRequired
        {
            get { return false; }
        }
        public bool IsVersionPropertyGenerated
        {
            get { return false; }
        }
        public bool HasProxy
        {
            get { return false; }
        }
        public bool HasCollections
        {
            get { return false; }
        }
        public bool HasMutableProperties
        {
            get { return false; }
        }
        public bool HasSubselectLoadableCollections
        {
            get { return false; }
        }
        public bool HasCascades
        {
            get { return false; }
        }
        public bool HasIdentifierProperty
        {
            get { return false; }
        }
        public bool CanExtractIdOutOfEntity
        {
            get { return false; }
        }
        public bool HasNaturalIdentifier
        {
            get { return false; }
        }
        public bool HasLazyProperties
        {
            get { return false; }
        }
        public bool[] PropertyUpdateability
        {
            get { return new bool[] {}; }
        }
        public bool HasCache
        {
            get { return false; }
        }
        public bool HasInsertGeneratedProperties
        {
            get { return false; }
        }
        public bool HasUpdateGeneratedProperties
        {
            get { return false; }
        }
        bool IOptimisticCacheSource.IsVersioned
        {
            get { return false; }
        }
        public IComparer VersionComparator
        {
            get { return null; }
        }
    }
}