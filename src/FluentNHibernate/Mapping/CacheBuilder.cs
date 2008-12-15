namespace FluentNHibernate.Mapping
{
    public class CacheBuilder<T>
    {
        private readonly ClassMap<T> parent;
        private string usage;

        public CacheBuilder(ClassMap<T> parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Cache with read-write usage.
        /// </summary>
        public ClassMap<T> AsReadWrite()
        {
            usage = "read-write";
            return parent;
        }

        /// <summary>
        /// Cache with nonstrict-read-write usage.
        /// </summary>
        /// <returns></returns>
        public ClassMap<T> AsNonStrictReadWrite()
        {
            usage = "nonstrict-read-write";
            return parent;
        }

        /// <summary>
        /// Cache with read-only usage.
        /// </summary>
        /// <returns></returns>
        public ClassMap<T> AsReadOnly()
        {
            usage = "read-only";
            return parent;
        }

        /// <summary>
        /// Cache with a custom usage, in place for forwards compatibility. 
        /// </summary>
        /// <param name="custom">Custom usage.</param>
        public ClassMap<T> AsCustom(string custom)
        {
            usage = custom;
            return parent;
        }

        /// <summary>
        /// Convert to a MappingPart.
        /// </summary>
        internal IMappingPart ToMappingPart()
        {
            return new CachePart(usage);
        }
    }
}