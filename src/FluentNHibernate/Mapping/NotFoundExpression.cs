namespace FluentNHibernate.Mapping
{
    public class NotFoundExpression<PARENTPART>
        where PARENTPART : IMappingPart
    {
        private readonly Cache<string, string> _properties;
        protected PARENTPART MappingPart { get; set; }

        public NotFoundExpression( PARENTPART mappingPart, Cache<string, string> properties )
        {
            MappingPart = mappingPart;
            _properties = properties;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        /// <returns></returns>
        public PARENTPART Ignore()
        {
            _properties.Store( "not-found", "ignore" );
            return MappingPart;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        /// <returns></returns>
        public PARENTPART Exception()
        {
            _properties.Store( "not-found", "exception" );
            return MappingPart;
        }
    }
}