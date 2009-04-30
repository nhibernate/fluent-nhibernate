using System;
using System.Collections.Generic;

namespace FluentNHibernate.Infrastructure
{
    public class Container
    {
        private readonly IDictionary<Type, Func<Container, object>> registeredTypes = new Dictionary<Type, Func<Container, object>>();

        public void Register<T>(Func<Container, object> instantiate)
        {
            registeredTypes.Add(typeof(T), instantiate);
        }

        public T Resolve<T>()
        {
            if (!registeredTypes.ContainsKey(typeof(T)))
                throw new InvalidOperationException("Can't resolve dependency: '" + typeof(T).FullName + "'");

            var instantiationFunc = registeredTypes[typeof(T)];

            return (T)instantiationFunc(this);
        }
    }
}