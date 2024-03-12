using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.Tuple.Entity;
using NHibernate.Type;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

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
        return Array.Empty<int>();
    }

    public int[] FindModified(object[] old, object[] current, object entity, ISessionImplementor session)
    {
        return Array.Empty<int>();
    }

    public object[] GetNaturalIdentifierSnapshot(object id, ISessionImplementor session)
    {
        return Array.Empty<object>();
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
        return Array.Empty<object>();
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
        return Array.Empty<object>();
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
        return Array.Empty<object>();
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

    public Task<int[]> FindDirtyAsync(object[] currentState, object[] previousState, object entity, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int[]> FindModifiedAsync(object[] old, object[] current, object entity, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object[]> GetNaturalIdentifierSnapshotAsync(object id, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object> LoadAsync(object id, object optionalObject, LockMode lockMode, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(object id, object[] fields, object obj, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object> InsertAsync(object[] fields, object obj, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(object id, object version, object obj, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object> ForceVersionIncrementAsync(object id, object currentVersion, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool?> IsTransientAsync(object obj, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void SetPropertyValues(object obj, object[] values)
    {
        throw new NotImplementedException();
    }

    public void SetPropertyValue(object obj, int i, object value)
    {
        throw new NotImplementedException();
    }

    public object[] GetPropertyValues(object obj)
    {
        throw new NotImplementedException();
    }

    public object GetPropertyValue(object obj, int i)
    {
        throw new NotImplementedException();
    }

    public object GetPropertyValue(object obj, string name)
    {
        throw new NotImplementedException();
    }

    public object GetIdentifier(object obj)
    {
        throw new NotImplementedException();
    }

    public void SetIdentifier(object obj, object id)
    {
        throw new NotImplementedException();
    }

    public object GetVersion(object obj)
    {
        throw new NotImplementedException();
    }

    public object Instantiate(object id)
    {
        throw new NotImplementedException();
    }

    public bool IsInstance(object entity)
    {
        throw new NotImplementedException();
    }

    public bool HasUninitializedLazyProperties(object obj)
    {
        throw new NotImplementedException();
    }

    public void ResetIdentifier(object entity, object currentId, object currentVersion)
    {
        throw new NotImplementedException();
    }

    public IEntityPersister GetSubclassEntityPersister(object instance, ISessionFactoryImplementor factory)
    {
        throw new NotImplementedException();
    }

    public ISessionFactoryImplementor Factory => null;

    public string RootEntityName => null;

    public string EntityName => null;

    public EntityMetamodel EntityMetamodel => null;

    public string[] PropertySpaces
    {
        get { return Array.Empty<string>(); }
    }
    public string[] QuerySpaces
    {
        get { return Array.Empty<string>(); }
    }
    public bool IsMutable => false;

    public bool IsInherited => false;

    public bool IsIdentifierAssignedByInsert => false;

    bool IEntityPersister.IsVersioned => false;

    public IVersionType VersionType => null;

    public int VersionProperty => 0;

    public int[] NaturalIdentifierProperties
    {
        get { return Array.Empty<int>(); }
    }
    public IIdentifierGenerator IdentifierGenerator => null;

    public IType[] PropertyTypes
    {
        get { return Array.Empty<IType>(); }
    }
    public string[] PropertyNames
    {
        get { return Array.Empty<string>(); }
    }
    public bool[] PropertyInsertability
    {
        get { return Array.Empty<bool>(); }
    }
    public ValueInclusion[] PropertyInsertGenerationInclusions
    {
        get { return Array.Empty<ValueInclusion>(); }
    }
    public ValueInclusion[] PropertyUpdateGenerationInclusions
    {
        get { return Array.Empty<ValueInclusion>(); }
    }
    public bool[] PropertyCheckability
    {
        get { return Array.Empty<bool>(); }
    }
    public bool[] PropertyNullability
    {
        get { return Array.Empty<bool>(); }
    }
    public bool[] PropertyVersionability
    {
        get { return Array.Empty<bool>(); }
    }
    public bool[] PropertyLaziness
    {
        get { return Array.Empty<bool>(); }
    }
    public CascadeStyle[] PropertyCascadeStyles
    {
        get { return Array.Empty<CascadeStyle>(); }
    }
    public IType IdentifierType => null;

    public string IdentifierPropertyName => null;

    public bool IsCacheInvalidationRequired => false;

    public bool IsLazyPropertiesCacheable => false;

    public ICacheConcurrencyStrategy Cache => null;

    public ICacheEntryStructure CacheEntryStructure => null;

    public IClassMetadata ClassMetadata => null;

    public bool IsBatchLoadable => false;

    public bool IsSelectBeforeUpdateRequired => false;

    public bool IsVersionPropertyGenerated => false;

    public bool HasProxy => false;

    public bool HasCollections => false;

    public bool HasMutableProperties => false;

    public bool HasSubselectLoadableCollections => false;

    public bool HasCascades => false;

    public bool HasIdentifierProperty => false;

    public bool CanExtractIdOutOfEntity => false;

    public bool HasNaturalIdentifier => false;

    public bool HasLazyProperties => false;

    public bool[] PropertyUpdateability
    {
        get { return Array.Empty<bool>(); }
    }
    public bool HasCache => false;

    public bool HasInsertGeneratedProperties => false;

    public bool HasUpdateGeneratedProperties => false;

    bool IOptimisticCacheSource.IsVersioned => false;

    public IComparer VersionComparator => null;

    bool IEntityPersister.IsInstrumented => throw new NotImplementedException();

    public Type MappedClass => throw new NotImplementedException();

    bool IEntityPersister.ImplementsLifecycle => throw new NotImplementedException();

    bool IEntityPersister.ImplementsValidatable => throw new NotImplementedException();

    public Type ConcreteProxyClass => throw new NotImplementedException();

    public EntityMode EntityMode => throw new NotImplementedException();

    public IEntityTuplizer EntityTuplizer => throw new NotImplementedException();
}
