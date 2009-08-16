using System;

namespace FluentNHibernate.Mapping
{
    public class CascadeExpression<TParent>
	{
        private readonly TParent parent;
        private readonly Action<string> setter;

        public CascadeExpression(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public TParent All()
		{
			setter("all");
			return parent;
		}

		public TParent None()
		{
			setter("none");
            return parent;
		}

		public TParent SaveUpdate()
		{
			setter("save-update");
            return parent;
		}

		public TParent Delete()
		{
			setter("delete");
            return parent;
		}
	}
}
