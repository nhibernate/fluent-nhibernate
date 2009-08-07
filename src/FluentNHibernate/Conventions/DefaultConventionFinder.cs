using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Default convention finder - doesn't do anything special.
    /// </summary>
    public class DefaultConventionFinder : IConventionFinder
    {
        private readonly ConventionsCollection conventions = new ConventionsCollection();

        public DefaultConventionFinder()
        {
            
        }

        /// <summary>
        /// Find any conventions implementing T.
        /// </summary>
        /// <typeparam name="T">Convention interface type</typeparam>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> Find<T>() where T : IConvention
        {
            foreach (var type in conventions.Where(x => typeof(T).IsAssignableFrom(x)))
            {
                foreach (var instance in conventions[type])
                {
                    yield return (T)instance;
                }
            }
        }

        /// <summary>
        /// Add an assembly to be queried.
        /// </summary>
        /// <remarks>
        /// All convention types must have a parameterless constructor, or a single parameter of IConventionFinder.
        /// </remarks>
        /// <param name="assembly">Assembly instance to query</param>
        public void AddAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsAbstract || type.IsGenericType || !typeof(IConvention).IsAssignableFrom(type)) continue;

                Add(type, MissingConstructor.Ignore);
            }
        }

        /// <summary>
        /// Adds all conventions found in the assembly that contains T.
        /// </summary>
        /// <remarks>
        /// All convention types must have a parameterless constructor, or a single parameter of IConventionFinder.
        /// </remarks>
        public void AddFromAssemblyOf<T>()
        {
            AddAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Add a single convention by type.
        /// </summary>
        /// <remarks>
        /// Type must have a parameterless constructor, or a single parameter of IConventionFinder.
        /// </remarks>
        /// <typeparam name="T">Convention type</typeparam>
        public void Add<T>() where T : IConvention
        {
            Add(typeof(T), MissingConstructor.Throw);
        }

        /// <summary>
        /// Add a single convention by type.
        /// </summary>
        /// <remarks>
        /// Types must have a parameterless constructor, or a single parameter of <see cref="IConventionFinder" />.
        /// </remarks>
        /// <param name="type">Type of convention</param>
        public void Add(Type type)
        {
            Add(type, MissingConstructor.Throw);
        }

        public void Add(Type type, object instance)
        {
            if (conventions.Contains(type) && !AllowMultiplesOf(type)) return;

            conventions.Add(type, instance);
        }

        /// <summary>
        /// Add an instance of a convention.
        /// </summary>
        /// <remarks>
        /// Useful for supplying conventions that require extra constructor parameters.
        /// </remarks>
        /// <typeparam name="T">Convention type</typeparam>
        /// <param name="instance">Instance of convention</param>
        public void Add<T>(T instance) where T : IConvention
        {
            if (conventions.Contains(typeof(T)) && !AllowMultiplesOf(instance.GetType())) return;

            conventions.Add(typeof(T), instance);
        }

        private void Add(Type type, MissingConstructor missingConstructor)
        {
            if (missingConstructor == MissingConstructor.Throw && !HasValidConstructor(type))
                throw new MissingConstructorException(type);
            if (missingConstructor == MissingConstructor.Ignore && !HasValidConstructor(type))
                return;

            if (conventions.Contains(type) && !AllowMultiplesOf(type)) return;

            conventions.Add(type, Instantiate(type));
        }

        private bool AllowMultiplesOf(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(MultipleAttribute), true) != null;
        }

        private object Instantiate(Type type)
        {
            object instance = null;

            // messy - find either ctor(IConventionFinder) or ctor()
            foreach (var constructor in type.GetConstructors())
            {
                if (IsFinderConstructor(constructor))
                    instance = constructor.Invoke(new[] { this });
                else if (IsParameterlessConstructor(constructor))
                    instance = constructor.Invoke(new object[] { });
            }

            return instance;
        }

        private bool HasValidConstructor(Type type)
        {
            foreach (var constructor in type.GetConstructors())
            {
                if (IsFinderConstructor(constructor) || IsParameterlessConstructor(constructor)) return true;
            }

            return false;
        }

        private bool IsFinderConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            return parameters.Length == 1 && parameters[0].ParameterType == typeof (IConventionFinder);
        }

        private bool IsParameterlessConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            return parameters.Length == 0;
        }

        private enum MissingConstructor
        {
            Throw,
            Ignore
        }
    }
}