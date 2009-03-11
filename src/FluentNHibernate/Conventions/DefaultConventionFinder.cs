using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.Conventions
{
    public class DefaultConventionFinder : IConventionFinder
    {
        private readonly IList<Type> addedTypes = new List<Type>();
        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        public IEnumerable<T> Find<T>() where T : IConvention
        {
            foreach (var type in addedTypes)
            {
                if (!typeof(T).IsAssignableFrom(type)) continue;
                
                yield return (T)instances[type];
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsAbstract || type.IsGenericType || !typeof(IConvention).IsAssignableFrom(type)) continue;

                Add(type);
            }
        }

        public void Add<T>() where T : IConvention
        {
            Add(typeof(T));
        }

        public void Add<T>(T instance) where T : IConvention
        {
            addedTypes.Add(typeof(T));
            instances[typeof(T)] = instance;
        }

        private void Add(Type type)
        {
            addedTypes.Add(type);
            instances[type] = Instantiate(type);
        }

        private object Instantiate(Type type)
        {
            var constructors = type.GetConstructors();
            object instance = null;

            // messy - find either ctor(IConventionFinder) or ctor()
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IConventionFinder))
                    instance = constructor.Invoke(new[] { this });
                else if (parameters.Length == 0)
                    instance = constructor.Invoke(new object[] { });
                else
                    throw new MissingConstructorException(type);
            }

            return instance;
        }
    }
}