using System;

namespace FluentNHibernate.Mapping
{
	public class FetchTypeExpression<TParent> 
	{
	    private readonly TParent parent;
	    private readonly Action<string> setter;

		public FetchTypeExpression(TParent parent, Action<string> setter)
		{
		    this.parent = parent;
		    this.setter = setter;
		}

        /// <summary>
        /// Join fetching
        /// </summary>
	    public TParent Join()
		{
		    setter("join");
            return parent;
		}

        /// <summary>
        /// Select fetching
        /// </summary>
		public TParent Select()
		{
		    setter("select");
            return parent;
		}

        /// <summary>
        /// Subselect/subquery fetching
        /// </summary>
        public TParent Subselect()
        {
            setter("subselect");
            return parent;
        }
	}
}
