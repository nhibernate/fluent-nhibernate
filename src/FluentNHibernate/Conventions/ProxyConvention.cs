using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public class ProxyConvention : IClassConvention, ISubclassConvention, IHasOneConvention, IReferenceConvention, ICollectionConvention
    {
        private readonly Func<Type, Type> _mapPersistentTypeToProxyInterfaceType;
        private readonly Func<Type, Type> _mapProxyInterfaceTypeToPersistentType;

        public ProxyConvention(
            Func<Type, Type> mapPersistentTypeToProxyInterfaceType,
            Func<Type, Type> mapProxyInterfaceTypeToPersistentType)
        {
            if (mapPersistentTypeToProxyInterfaceType == null)
            {
                throw new ArgumentNullException("mapPersistentTypeToProxyInterfaceType");
            }

            if(mapProxyInterfaceTypeToPersistentType == null)
            {
                throw new ArgumentNullException("mapProxyInterfaceTypeToPersistentType");
            }

            this._mapPersistentTypeToProxyInterfaceType = mapPersistentTypeToProxyInterfaceType;
            
            this._mapProxyInterfaceTypeToPersistentType = mapProxyInterfaceTypeToPersistentType;
        }

        /// <summary>
        /// Apply changes to the target
        /// </summary>
        public void Apply(IClassInstance instance)
        {
            var proxy = GetProxyType(instance.EntityType);

            if(proxy != null)
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

            if(proxy != null)
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
            Type persistentType = _mapProxyInterfaceTypeToPersistentType(inferredType);

            if (persistentType != null)
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

            if(proxy != null)
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
            Type persistentType = _mapProxyInterfaceTypeToPersistentType(inferredType);

            if(persistentType != null)
            {
                instance.OverrideInferredClass(persistentType);
            }
        }

        private Type GetProxyType(Type persistentType)
        {
            return !persistentType.IsAbstract
                       ? _mapPersistentTypeToProxyInterfaceType(persistentType)
                       : null;
        }

        private Type GetPersistentType(Type proxyType)
        {
            return proxyType.IsInterface
                       ? _mapProxyInterfaceTypeToPersistentType(proxyType)
                       : null;
        }
    }
}
