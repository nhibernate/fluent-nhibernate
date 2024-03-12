using System;

namespace FluentNHibernate.Conventions.Helpers.Prebuilt;

public class BuiltProxyConvention : ProxyConvention
{
    public BuiltProxyConvention(Type proxyType, Type persistentType)
        :this( 
            type => type == persistentType ? proxyType : null,
            type => type == proxyType ? persistentType : null
        )
    {}

    protected BuiltProxyConvention(Func<Type, Type> mapPersistentTypeToProxyInterfaceType, Func<Type, Type> mapProxyInterfaceTypeToPersistentType) : base(mapPersistentTypeToProxyInterfaceType, mapProxyInterfaceTypeToPersistentType)
    {
    }
}
