using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions;

public class ProxyConvention(Func<Type, Type> mapPersistentTypeToProxyInterfaceType, Func<Type, Type> mapProxyInterfaceTypeToPersistentType)
    : IClassConvention, ISubclassConvention, IHasOneConvention, IReferenceConvention, ICollectionConvention
{
    readonly Func<Type, Type> mapPersistentTypeToProxyInterfaceType = mapPersistentTypeToProxyInterfaceType ?? throw new ArgumentNullException(nameof(mapPersistentTypeToProxyInterfaceType));
    readonly Func<Type, Type> mapProxyInterfaceTypeToPersistentType = mapProxyInterfaceTypeToPersistentType ?? throw new ArgumentNullException(nameof(mapProxyInterfaceTypeToPersistentType));

    /// <summary>
    /// Apply changes to the target
    /// </summary>
    public void Apply(IClassInstance instance)
    {
        var proxy = GetProxyType(instance.EntityType);

        if(proxy is not null)
        {
            instance.Proxy(proxy);
        }
    }

    /// <summary>
    /// Apply changes to the target
    /// </summary>
    public void Apply(ISubclassInstance instance)
    {
        var proxy = GetProxyType(instance.EntityType);

        if(proxy is not null)
        {
            instance.Proxy(proxy);
        }
    }

    /// <summary>
    /// Apply changes to the target
    /// </summary>
    public void Apply(IManyToOneInstance instance)
    {
        Type inferredType = instance.Class.GetUnderlyingSystemType();
        Type persistentType = mapProxyInterfaceTypeToPersistentType(inferredType);

        if (persistentType is not null)
        {
            instance.OverrideInferredClass(persistentType);
        }
    }

    /// <summary>
    /// Apply changes to the target
    /// </summary>
    public void Apply(ICollectionInstance instance)
    {
        var proxy = GetPersistentType(instance.Relationship.Class.GetUnderlyingSystemType());

        if(proxy is not null)
        {
            instance.Relationship.CustomClass(proxy);
        }
    }

    /// <summary>
    /// Apply changes to the target
    /// </summary>
    public void Apply(IOneToOneInstance instance)
    {
        Type inferredType = ((IOneToOneInspector)instance).Class.GetUnderlyingSystemType();
        Type persistentType = mapProxyInterfaceTypeToPersistentType(inferredType);

        if(persistentType is not null)
        {
            instance.OverrideInferredClass(persistentType);
        }
    }

    private Type GetProxyType(Type persistentType)
    {
        return !persistentType.IsAbstract
            ? mapPersistentTypeToProxyInterfaceType(persistentType)
            : null;
    }

    private Type GetPersistentType(Type proxyType)
    {
        return proxyType.IsInterface
            ? mapProxyInterfaceTypeToPersistentType(proxyType)
            : null;
    }
}
