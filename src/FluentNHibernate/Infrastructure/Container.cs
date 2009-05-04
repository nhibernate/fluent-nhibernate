using System;
using System.Collections.Generic;

namespace FluentNHibernate.Infrastructure
{
    public class Container
    {
        private readonly IDictionary<Type, Func<Container, object>> registeredTypes = new Dictionary<Type, Func<Container, object>>();
        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        public void Register<T>(Func<Container, object> instantiateFunc)
        {
            registeredTypes[typeof(T)] = instantiateFunc;
        }

        public object Resolve(Type type)
        {
            if (!registeredTypes.ContainsKey(type))
                throw new ResolveException(type);
            
            if (!instances.ContainsKey(type))
            {
                var instantiationFunc = registeredTypes[type];

                instances[type] = instantiationFunc(this);
            }

            return instances[type];
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
    }
}