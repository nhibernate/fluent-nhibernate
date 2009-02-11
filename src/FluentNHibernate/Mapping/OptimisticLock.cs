namespace FluentNHibernate.Mapping
{
    public class OptimisticLock
    {
        private const string AttributeKey = "optimistic-lock";
        private readonly Cache<string, string> attributes;

        public OptimisticLock(Cache<string, string> attributes)
        {
            this.attributes = attributes;
        }

        /// <summary>
        /// Use no locking strategy
        /// </summary>
        public void None()
        {
            attributes.Store(AttributeKey, "none");
        }

        /// <summary>
        /// Use version locking
        /// </summary>
        public void Version()
        {
            attributes.Store(AttributeKey, "version");
        }

        /// <summary>
        /// Use dirty locking
        /// </summary>
        public void Dirty()
        {
            attributes.Store(AttributeKey, "dirty");
        }

        /// <summary>
        /// Use all locking
        /// </summary>
        public void All()
        {
            attributes.Store(AttributeKey, "all");
        }
    }
}