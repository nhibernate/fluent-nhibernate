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

        /// <summary>
        /// Cascade all actions
        /// </summary>
        public TParent All()
		{
			setter("all");
			return parent;
		}

        /// <summary>
        /// Cascade no actions
        /// </summary>
		public TParent None()
		{
			setter("none");
            return parent;
		}

        /// <summary>
        /// Cascade saves and updates
        /// </summary>
		public TParent SaveUpdate()
		{
			setter("save-update");
            return parent;
		}

        /// <summary>
        /// Cascade deletes
        /// </summary>
		public TParent Delete()
		{
			setter("delete");
            return parent;
		}

        /// <summary>
        /// Cascade deletes
        /// </summary>
        public TParent Merge()
        {
            setter("merge");
            return parent;
        }


	}
}
