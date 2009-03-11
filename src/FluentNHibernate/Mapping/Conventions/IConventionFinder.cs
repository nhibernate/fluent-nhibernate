using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentNHibernate.Mapping.Conventions
{
    public interface IConventionFinder
    {
        void AddAssembly(Assembly assembly);
        IEnumerable<T> Find<T>();
    }

    public class DefaultConventionFinder : IConventionFinder
    {
        private readonly IList<Type> addedTypes = new List<Type>();
        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        public IEnumerable<T> Find<T>()
        {
            // messy - find either ctor(IConventionFinder) or ctor()
            foreach (var type in addedTypes)
            {
                if (!typeof(T).IsAssignableFrom(type)) continue;
                if (instances.ContainsKey(type)) yield return (T)instances[type];

                var constructors = type.GetConstructors();
                T instance = default(T);

                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();

                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IConventionFinder))
                        instance = (T)constructor.Invoke(new[] { this });
                    else if (parameters.Length == 0)
                        instance = (T)constructor.Invoke(new object[] {});
                    else
                        throw new MissingConstructorException(type);
                }

                instances[type] = instance;
                yield return instance;
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsAbstract || type.IsGenericType) continue;

                addedTypes.Add(type);
            }
        }
    }
}