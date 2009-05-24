using System;

namespace FluentNHibernate.Mapping
{
    public class OuterJoinBuilder<TParent>
    {
        private readonly TParent parent;
        private readonly Action<string> setter;

        public OuterJoinBuilder(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public TParent Auto()
        {
            setter("auto");
            return parent;
        }

        public TParent Yes()
        {
            setter("true");
            return parent;
        }

        public TParent No()
        {
            setter("false");
            return parent;
        }
    }
}