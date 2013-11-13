using NHibernate.Cfg;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Fluently configure NHibernate
    /// </summary>
    public class Fluently
    {
        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <returns>Fluent Configuration</returns>
        public FluentConfiguration BeginConfigure()
        {
            return Fluently.Configure();
        }
        
        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <returns>Fluent Configuration</returns>
        public static FluentConfiguration Configure()
        {
            return new FluentConfiguration();
        }
        
        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <param name="cfg">Instance of an NHibernate Configuration</param>
        /// <returns>Fluent Configuration</returns>
        public FluentConfiguration BeginConfigure(Configuration cfg)
        {
            return Fluently.Configure(cfg);
        }

        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <param name="cfg">Instance of an NHibernate Configuration</param>
        /// <returns>Fluent Configuration</returns>
        public static FluentConfiguration Configure(Configuration cfg)
        {
            return new FluentConfiguration(cfg);
        }
    }
}
