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
        private readonly IList<Assembly> assemblies = new List<Assembly>();

        public IEnumerable<T> Find<T>()
        {
            var types = from assembly in assemblies
                        from type in assembly.GetExportedTypes()
                        where typeof(T).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericType
                        select type;

            // messy - find either ctor(IConventionFinder) or ctor()
            foreach (var type in types)
            {
                var constructors = type.GetConstructors();

                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();

                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IConventionFinder))
                        yield return (T)constructor.Invoke(new[] { this });
                    else if (parameters.Length == 0)
                        yield return (T)constructor.Invoke(new object[] {});
                    else
                        throw new MissingConstructorException(type);
                }
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
        }
    }
}