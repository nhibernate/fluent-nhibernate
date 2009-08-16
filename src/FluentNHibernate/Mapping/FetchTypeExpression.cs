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

	    public TParent Join()
		{
		    setter("join");
            return parent;
		}

		public TParent Select()
		{
		    setter("select");
            return parent;
		}
	}
}
