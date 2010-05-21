using System;

namespace FluentNHibernate.Mapping
{
    public class CheckTypeExpression<TParent>
    {
        internal Action<string> setValue;

        private readonly TParent parent;

        public CheckTypeExpression(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setValue = setter;
        }

        public void None()
        {
            setValue("none");
        }

        public void RowCount()
        {
            setValue("rowcount");
        }
    }
}
