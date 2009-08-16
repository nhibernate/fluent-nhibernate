using System;

namespace FluentNHibernate.Mapping
{
    public class VersionGeneratedBuilder<TParent>
    {
        private readonly TParent parent;
        private readonly Action<string> setter;

        public VersionGeneratedBuilder(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public TParent Always()
        {
            setter("always");
            return parent;
        }

        public TParent Never()
        {
            setter("never");
            return parent;
        }
    }
}