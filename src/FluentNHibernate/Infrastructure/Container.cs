using System;
using System.Collections.Generic;

namespace FluentNHibernate.Infrastructure;

public class Container
{
    readonly Dictionary<Type, Func<Container, object>> registeredTypes = new();

    public void Register<T>(Func<Container, object> instantiateFunc)
    {
        registeredTypes[typeof(T)] = instantiateFunc;
    }

    public object Resolve(Type type)
    {
        if (!registeredTypes.TryGetValue(type, out var instantiationFunc))
            throw new ResolveException(type);

        return instantiationFunc(this);
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }
}
