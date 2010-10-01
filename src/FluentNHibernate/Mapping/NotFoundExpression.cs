using System;
using FluentNHibernate.Mapping.Builders;

namespace FluentNHibernate.Mapping
{
    public class NotFoundExpression<TParent>
    {
        private readonly TParent parent;
        readonly NotFoundBuilder builder;

        public NotFoundExpression(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            builder = new NotFoundBuilder(setter);
        }

        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        public TParent Ignore()
        {
            builder.Ignore();
            return parent;
        }

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        public TParent Exception()
        {
            builder.Exception();
            return parent;
        }
    }
}
