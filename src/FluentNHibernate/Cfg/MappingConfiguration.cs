using System;
using NHibernate.Cfg;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Fluent mapping configuration
    /// </summary>
    public class MappingConfiguration
    {
        private bool mergeMappings;

        public MappingConfiguration()
        {
            FluentMappings = new FluentMappingsContainer();
            AutoMappings = new AutoMappingsContainer();
            HbmMappings = new HbmMappingsContainer();
        }

        /// <summary>
        /// Fluent mappings
        /// </summary>
        public FluentMappingsContainer FluentMappings { get; private set; }

        /// <summary>
        /// Automatic mapping configurations
        /// </summary>
        public AutoMappingsContainer AutoMappings { get; private set; }

        /// <summary>
        /// Hbm mappings
        /// </summary>
        public HbmMappingsContainer HbmMappings { get; private set; }

        /// <summary>
        /// Get whether any mappings of any kind were added
        /// </summary>
        public bool WasUsed
        {
            get
            {
                return FluentMappings.WasUsed ||
                       AutoMappings.WasUsed ||
                       HbmMappings.WasUsed;
            }
        }

        /// <summary>
        /// Applies any mappings to the NHibernate Configuration
        /// </summary>
        /// <param name="cfg">NHibernate Configuration instance</param>
        public void Apply(Configuration cfg)
        {
            if (mergeMappings)
            {
                foreach (var model in AutoMappings)
                    model.MergeMappings = true;

                FluentMappings.PersistenceModel.MergeMappings = true;
            }

            HbmMappings.Apply(cfg);
            FluentMappings.Apply(cfg);
            AutoMappings.Apply(cfg);
        }

        public MappingConfiguration MergeMappings()
        {
            mergeMappings = true;

            return this;
        }
    }
}