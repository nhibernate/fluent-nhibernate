using System;

namespace FluentNHibernate.Mapping.Builders
{
    public class FetchTypeBuilder
    {
        private readonly Action<string> setter;

        public FetchTypeBuilder(Action<string> setter)
        {
            this.setter = setter;
        }

        /// <summary>
        /// Join fetching
        /// </summary>
        public void Join()
        {
            setter("join");
        }

        /// <summary>
        /// Select fetching
        /// </summary>
        public void Select()
        {
            setter("select");
        }

        /// <summary>
        /// Subselect/subquery fetching
        /// </summary>
        public void Subselect()
        {
            setter("subselect");
        }
    }
}