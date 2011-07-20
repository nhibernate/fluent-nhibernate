using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentNHibernate.Automapping
{
    /// <summary>
    /// Starting point for automapping your entities.
    /// </summary>
    public static class AutoMap
    {
        /// <summary>
        /// Automatically map classes in the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Class in the assembly you want to map</typeparam>
        public static AutoPersistenceModel AssemblyOf<T>()
        {
            return Assembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Automatically map classes in the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Class in the assembly you want to map</typeparam>
        /// <param name="cfg">Automapping configuration</param>
        public static AutoPersistenceModel AssemblyOf<T>(IAutomappingConfiguration cfg)
        {
            return Assembly(typeof(T).Assembly, cfg);
        }

        /// <summary>
        /// Automatically map the classes in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly containing the classes to map</param>
        public static AutoPersistenceModel Assembly(Assembly assembly)
        {
            return Source(new AssemblyTypeSource(assembly));
        }

        /// <summary>
        /// Automatically map the classes in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly containing the classes to map</param>
        /// <param name="cfg">Automapping configuration</param>
        public static AutoPersistenceModel Assembly(Assembly assembly, IAutomappingConfiguration cfg)
        {
            return Source(new AssemblyTypeSource(assembly), cfg);
        }

        /// <summary>
        /// Automatically map the classes in each assembly supplied.
        /// </summary>
        /// <param name="assemblies">Assemblies containing classes to map</param>
        public static AutoPersistenceModel Assemblies(params Assembly[] assemblies)
        {
            return Source(new CombinedAssemblyTypeSource(assemblies.Select(x => new AssemblyTypeSource(x))));
        }

        /// <summary>
        /// Automatically map the classes in each assembly supplied.
        /// </summary>
        /// <param name="cfg">Automapping configuration</param>
        /// <param name="assemblies">Assemblies containing classes to map</param>
        public static AutoPersistenceModel Assemblies(IAutomappingConfiguration cfg, params Assembly[] assemblies)
        {
            return Source(new CombinedAssemblyTypeSource(assemblies.Select(x => new AssemblyTypeSource(x))), cfg);
        }

        /// <summary>
        /// Automatically map the classes in each assembly supplied.
        /// </summary>
        /// <param name="cfg">Automapping configuration</param>
        /// <param name="assemblies">Assemblies containing classes to map</param>
        public static AutoPersistenceModel Assemblies(IAutomappingConfiguration cfg, IEnumerable<Assembly> assemblies)
        {
            return Source(new CombinedAssemblyTypeSource(assemblies.Select(x => new AssemblyTypeSource(x))), cfg);
        }

        /// <summary>
        /// Automatically map the classes exposed through the supplied <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> containing classes to map</param>
        public static AutoPersistenceModel Source(ITypeSource source)
        {
            return new AutoPersistenceModel()
                .AddTypeSource(source);
        }

        /// <summary>
        /// Automatically map the classes exposed through the supplied <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> containing classes to map</param>
        /// <param name="cfg">Automapping configuration</param>
        public static AutoPersistenceModel Source(ITypeSource source, IAutomappingConfiguration cfg)
        {
            return new AutoPersistenceModel(cfg)
                .AddTypeSource(source);
        }

        #region Depreciated overloads

        /// <summary>
        /// Automatically map the classes exposed through the supplied <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> containing classes to map</param>
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
        public static AutoPersistenceModel Source(ITypeSource source, Func<Type, bool> where)
        {
            return Source(source)
                .Where(where);
        }

        /// <summary>
        /// Automatically map the classes in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly containing the classes to map</param>
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
        public static AutoPersistenceModel Assembly(Assembly assembly, Func<Type, bool> where)
        {
            return Source(new AssemblyTypeSource(assembly))
                .Where(where);
        }

        /// <summary>
        /// Automatically map classes in the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Class in the assembly you want to map</typeparam>
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
        public static AutoPersistenceModel AssemblyOf<T>(Func<Type, bool> where)
        {
            return Assembly(typeof(T).Assembly)
                .Where(where);
        }

        #endregion
    }
}