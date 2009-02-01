using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Container for Hbm mappings
    /// </summary>
    public class HbmMappingsContainer
    {
        private readonly IList<Type> classes = new List<Type>();
        private readonly IList<Assembly> assemblies = new List<Assembly>();

        internal HbmMappingsContainer()
        {}

        /// <summary>
        /// Add explicit classes with Hbm mappings
        /// </summary>
        /// <param name="types">List of types to map</param>
        /// <returns>Hbm mappings configuration</returns>
        public HbmMappingsContainer AddClasses(params Type[] types)
        {
            foreach (var type in types)
            {
                classes.Add(type);
            }

            WasUsed = (types.Length > 0);
            return this;
        }

        /// <summary>
        /// Add all Hbm mappings in the assembly that contains T.
        /// </summary>
        /// <typeparam name="T">Type from the assembly</typeparam>
        /// <returns>Hbm mappings configuration</returns>
        public HbmMappingsContainer AddFromAssemblyOf<T>()
        {
            return AddFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Add all Hbm mappings in the assembly
        /// </summary>
        /// <param name="assembly">Assembly to add mappings from</param>
        /// <returns>Hbm mappings configuration</returns>
        public HbmMappingsContainer AddFromAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
            WasUsed = true;
            return this;
        }

        /// <summary>
        /// Gets whether any mappings were added
        /// </summary>
        internal bool WasUsed { get; set; }

        /// <summary>
        /// Applies any added mappings to the NHibernate Configuration
        /// </summary>
        /// <param name="cfg">NHibernate Configuration instance</param>
        internal void Apply(Configuration cfg)
        {
            foreach (var persistentClass in classes)
            {
                cfg.AddClass(persistentClass);
            }

            foreach (var assembly in assemblies)
            {
                cfg.AddAssembly(assembly);
            }
        }
    }
}