using System;
using FluentNHibernate.Mapping.Builders;

namespace FluentNHibernate.Mapping
{
    public class FetchTypeExpression<TParent> 
	{
	    private readonly TParent parent;
        readonly FetchTypeBuilder builder;

        public FetchTypeExpression(TParent parent, Action<string> setter)
		{
		    this.parent = parent;
		    builder = new FetchTypeBuilder(setter);
		}

        /// <summary>
        /// Join fetching
        /// </summary>
	    public TParent Join()
		{
            builder.Join();
            return parent;
		}

        /// <summary>
        /// Select fetching
        /// </summary>
		public TParent Select()
		{
            builder.Select();
            return parent;
		}

        /// <summary>
        /// Subselect/subquery fetching
        /// </summary>
        public TParent Subselect()
        {
            builder.Subselect();
            return parent;
        }
	}
}
