using System;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using System.IO;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Container for automatic mappings
    /// </summary>
    public class AutoMappingsContainer : IEnumerable<AutoPersistenceModel>
    {
        private readonly IList<AutoPersistenceModel> mappings = new List<AutoPersistenceModel>();
        private string exportPath;
        private TextWriter exportTextWriter;

        internal AutoMappingsContainer()
        {}

        /// <summary>
        /// Add automatic mappings
        /// </summary>
        /// <param name="model">Lambda returning an auto mapping setup</param>
        /// <returns>Auto mappings configuration</returns>
        public AutoMappingsContainer Add(Func<AutoPersistenceModel> model)
        {
            return Add(model());
        }

        /// <summary>
        /// Add automatic mappings
        /// </summary>
        /// <param name="model">Auto mapping setup</param>
        /// <returns>Auto mappings configuration</returns>
        public AutoMappingsContainer Add(AutoPersistenceModel model)
        {
            mappings.Add(model);
            WasUsed = true;
            return this;
        }

        /// <summary>
        /// Sets the export location for generated mappings
        /// </summary>
        /// <param name="path">Path to folder for mappings</param>
        /// <returns>Auto mappings configuration</returns>
        public AutoMappingsContainer ExportTo(string path)
        {
            exportPath = path;
            return this;
        }

        /// <summary>
        /// Sets the text writer to write the generated mappings to.
        /// </summary>                
        /// <returns>Fluent mappings configuration</returns>
        public AutoMappingsContainer ExportTo(TextWriter textWriter)
        {
            exportTextWriter = textWriter;
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
        /// <param name="model"></param>
        internal void Apply(Configuration cfg, PersistenceModel model)
        {
            foreach (var mapping in mappings)
            {
                if (!string.IsNullOrEmpty(exportPath))
                    mapping.WriteMappingsTo(exportPath);

                if (exportTextWriter != null)
                    mapping.WriteMappingsTo(exportTextWriter);

                mapping.ImportProviders(model);
                mapping.Configure(cfg);
            }
        }

        public IEnumerator<AutoPersistenceModel> GetEnumerator()
        {
            return mappings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}