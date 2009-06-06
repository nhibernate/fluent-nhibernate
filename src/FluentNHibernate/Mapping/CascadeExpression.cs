using System;

namespace FluentNHibernate.Mapping
{
    public class CascadeExpression<TParent> : ICascadeExpression
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

        void ICascadeExpression.All()
        {
            All();
        }

		public TParent None()
		{
			setter("none");
            return parent;
		}

        void ICascadeExpression.None()
        {
            None();
        }

		public TParent SaveUpdate()
		{
			setter("save-update");
            return parent;
		}

        void ICascadeExpression.SaveUpdate()
        {
            SaveUpdate();
        }

		public TParent Delete()
		{
			setter("delete");
            return parent;
		}

        void ICascadeExpression.Delete()
        {
            Delete();
        }
	}
}
