using NHibernate.Cfg;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Fluent mapping configuration
    /// </summary>
    public class MappingConfiguration
    {
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
            HbmMappings.Apply(cfg);
            FluentMappings.Apply(cfg);
            AutoMappings.Apply(cfg);
        }
    }
}