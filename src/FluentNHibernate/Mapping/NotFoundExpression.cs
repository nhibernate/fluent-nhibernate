namespace FluentNHibernate.Mapping
{
    public class NotFoundExpression<TParentpart> : INotFoundExpression where TParentpart : IHasAttributes
    {
        private readonly Cache<string, string> properties;
        protected TParentpart MappingPart { get; set; }

        public NotFoundExpression( TParentpart mappingPart, Cache<string, string> properties )
        {
            MappingPart = mappingPart;
            this.properties = properties;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        /// <returns></returns>
        public TParentpart Ignore()
        {
            properties.Store( "not-found", "ignore" );
            return MappingPart;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        void INotFoundExpression.Ignore()
        {
            Ignore();
        }

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        /// <returns></returns>
        public TParentpart Exception()
        {
            properties.Store( "not-found", "exception" );
            return MappingPart;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        void INotFoundExpression.Exception()
        {
            Exception();
        }
    }
}
