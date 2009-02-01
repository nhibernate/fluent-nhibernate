namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Fluently configure NHibernate
    /// </summary>
    public static class Fluently
    {
        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <returns>Fluent Configuration</returns>
        public static FluentConfiguration Configure()
        {
            return new FluentConfiguration();
        }
    }
}