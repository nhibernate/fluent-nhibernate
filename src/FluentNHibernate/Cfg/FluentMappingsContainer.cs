using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Visitors;
using System.IO;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Container for fluent mappings
    /// </summary>
    public class FluentMappingsContainer
    {
        readonly IList<Assembly> assemblies = new List<Assembly>();
        readonly List<Type> types = new List<Type>();
        readonly IConventionFinder conventionFinder = new DefaultConventionFinder();
        string exportPath;
        TextWriter exportTextWriter;
        PairBiDirectionalManyToManySidesDelegate biDirectionalManyToManyPairer;

        [Obsolete("PersistenceModel is no longer available through FluentMappingsContainer. Use MappingConfiguration.UsePersistenceModel to supply a custom PersistenceModel", true)]
        public PersistenceModel PersistenceModel
        {
            get { return null; }
        }

        public FluentMappingsContainer OverrideBiDirectionalManyToManyPairing(PairBiDirectionalManyToManySidesDelegate userControlledPairing)
        {
            biDirectionalManyToManyPairer = userControlledPairing;
            return this;
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
        /// Adds a single <see cref="IMappingProvider" /> represented by the specified type.
        /// </summary>
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer Add<T>()
        {
            return Add(typeof(T));
        }

        /// <summary>
        /// Adds a single <see cref="IMappingProvider" /> represented by the specified type.
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
        /// Sets the text writer to write the generated mappings to.
        /// </summary>                
        /// <returns>Fluent mappings configuration</returns>
        public FluentMappingsContainer ExportTo(TextWriter textWriter)
        {
            exportTextWriter = textWriter;
            return this;
        }

        /// <summary>
        /// Alter convention discovery
        /// </summary>
        public SetupConventionFinder<FluentMappingsContainer> Conventions
        {
            get { return new SetupConventionFinder<FluentMappingsContainer>(this, conventionFinder); }
        }

        /// <summary>
        /// Gets whether any mappings were added
        /// </summary>
        internal bool WasUsed { get; set; }

        /// <summary>
        /// Applies any added mappings to the NHibernate Configuration
        /// </summary>
        /// <param name="model">PersistenceModel to alter</param>
        internal void Apply(PersistenceModel model)
        {
            foreach (var assembly in assemblies)
            {
                model.AddMappingsFromAssembly(assembly);
            }

            foreach (var type in types)
            {
                model.Add(type);
            }

            if (!string.IsNullOrEmpty(exportPath))
                model.WriteMappingsTo(exportPath);

            if (exportTextWriter != null)
                model.WriteMappingsTo(exportTextWriter);

            if (biDirectionalManyToManyPairer != null)
                model.BiDirectionalManyToManyPairer = biDirectionalManyToManyPairer;

            model.Conventions.Merge(conventionFinder);
        }
    }
}