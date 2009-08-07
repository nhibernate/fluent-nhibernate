using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention finder - used to search through assemblies for types that implement a specific convention interface.
    /// </summary>
    public interface IConventionFinder
    {
        /// <summary>
        /// Add an assembly to be queried.
        /// </summary>
        /// <remarks>
        /// All convention types must have a parameterless constructor, or a single parameter of <see cref="IConventionFinder" />.
        /// </remarks>
        /// <param name="assembly">Assembly instance to query</param>
        void AddAssembly(Assembly assembly);

        /// <summary>
        /// Adds all conventions found in the assembly that contains <typeparam name="T" />.
        /// </summary>
        /// <remarks>
        /// All convention types must have a parameterless constructor, or a single parameter of <see cref="IConventionFinder" />.
        /// </remarks>
        void AddFromAssemblyOf<T>();

        /// <summary>
        /// Add a single convention by type.
        /// </summary>
        /// <remarks>
        /// Type must have a parameterless constructor, or a single parameter of <see cref="IConventionFinder" />.
        /// </remarks>
        /// <typeparam name="T">Convention type</typeparam>
        void Add<T>() where T : IConvention;

        /// <summary>
        /// Add a single convention by type.
        /// </summary>
        /// <remarks>
        /// Types must have a parameterless constructor, or a single parameter of <see cref="IConventionFinder" />.
        /// </remarks>
        /// <param name="type">Type of convention</param>
        void Add(Type type);

        void Add(Type type, object instance);

        /// <summary>
        /// Add an instance of a convention.
        /// </summary>
        /// <remarks>
        /// Useful for supplying conventions that require extra constructor parameters.
        /// </remarks>
        /// <typeparam name="T">Convention type</typeparam>
        /// <param name="instance">Instance of convention</param>
        void Add<T>(T instance) where T : IConvention;

        /// <summary>
        /// Find any conventions implementing T.
        /// </summary>
        /// <typeparam name="T">Convention interface type</typeparam>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> Find<T>() where T : IConvention;
    }
}