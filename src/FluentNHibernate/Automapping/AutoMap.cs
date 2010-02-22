using System;
using System.Collections.Generic;
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
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        public static AutoPersistenceModel AssemblyOf<T>(Func<Type, bool> where)
        {
            return Assembly(typeof(T).Assembly, where);
        }

        /// <summary>
        /// Automatically map the classes in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly containing the classes to map</param>
        public static AutoPersistenceModel Assembly(Assembly assembly)
        {
            return Assembly(assembly, null);
        }

        /// <summary>
        /// Automatically map the classes in <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly containing the classes to map</param>
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        public static AutoPersistenceModel Assembly(Assembly assembly, Func<Type, bool> where)
        {
            return Source(new AssemblyTypeSource(assembly), where);
        }

        /// <summary>
        /// Automatically map the classes in each assembly supplied.
        /// </summary>
        /// <param name="assemblies">Assemblies containing classes to map</param>
        public static AutoPersistenceModel Assemblies(params Assembly[] assemblies)
        {
            return Source(new CombinedAssemblyTypeSource(assemblies));
        }

        /// <summary>
        /// Automatically map the classes in each assembly supplied.
        /// </summary>
        /// <param name="assemblies">Assemblies containing classes to map</param>
        public static AutoPersistenceModel Assemblies(IEnumerable<Assembly> assemblies)
        {
            return Source(new CombinedAssemblyTypeSource(assemblies));
        }

        /// <summary>
        /// Automatically map the classes exposed through the supplied <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> containing classes to map</param>
        public static AutoPersistenceModel Source(ITypeSource source)
        {
            return Source(source, null);
        }

        /// <summary>
        /// Automatically map the classes exposed through the supplied <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> containing classes to map</param>
        /// <param name="where">Criteria for selecting a subset of the types in the assembly for mapping</param>
        public static AutoPersistenceModel Source(ITypeSource source, Func<Type, bool> where)
        {
            var persistenceModel = new AutoPersistenceModel();

            persistenceModel.AddTypeSource(source);

            if (where != null)
                persistenceModel.Where(where);

            return persistenceModel;
        }
    }
}