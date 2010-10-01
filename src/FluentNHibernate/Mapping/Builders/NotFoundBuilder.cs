using System;

namespace FluentNHibernate.Mapping.Builders
{
    public class NotFoundBuilder
    {
        private readonly Action<string> setter;

        public NotFoundBuilder(Action<string> setter)
        {
            this.setter = setter;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        public void Ignore()
        {
            setter("ignore");
        }

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        public void Exception()
        {
            setter("exception");
        }
    }
}