using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Container for fluent mappings
    /// </summary>
    public class FluentMappingsContainer
    {
        private readonly IList<Assembly> assemblies = new List<Assembly>();
        protected List<Type> types = new List<Type>();
        private string exportPath;
        private readonly PersistenceModel model;

        internal FluentMappingsContainer()
        {
            model = new PersistenceModel();
        }

        /// <summary>
        /// Add all fluent mappings in the assembly that contains T.
        /// </summary>
        /// <typeparam name="T">Type from the assembly</typeparam>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer AddFromAssemblyOf<T>()
        {
            return AddFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Add all fluent mappings in the assembly
        /// </summary>
        /// <param name="assembly">Assembly to add mappings from</param>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer AddFromAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
            WasUsed = true;
            return this;
		}

        /// <summary>
        /// Adds a single <see cref="IClassMap" /> represented by the specified type.
        /// </summary>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer Add<T>() where T : IClassMap
        {
            return Add(typeof(T));
        }

        /// <summary>
        /// Adds a single <see cref="IClassMap" /> represented by the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer Add(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            types.Add(type);
            WasUsed = true;
            return this;
        }

		/// <summary>
        /// Sets the export location for generated mappings
        /// </summary>
        /// <param name="path">Path to folder for mappings</param>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer ExportTo(string path)
        {
            exportPath = path;
            return this;
        }

        /// <summary>
        /// Alter convention discovery
        /// </summary>
        public SetupConventionFinder<FluentMappingsContainer> ConventionDiscovery
        {
            get { return new SetupConventionFinder<FluentMappingsContainer>(this, model.ConventionFinder); }
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
            foreach (var assembly in assemblies)
            {
                model.addMappingsFromAssembly(assembly);
            }

            foreach (var type in types)
            {
                model.AddMapping(type);
            }

            if (!string.IsNullOrEmpty(exportPath))
                model.WriteMappingsTo(exportPath);

            model.Configure(cfg);
        }
    }
}