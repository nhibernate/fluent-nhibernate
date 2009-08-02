using System;

namespace FluentNHibernate.Mapping
{
    public class PolymorphismBuilder<T>
    {
        private readonly T parent;
        private readonly Action<string> setter;

        public PolymorphismBuilder(T parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public T Implicit()
        {
            setter("implicit");
            return parent;
        }

        public T Explicit()
        {
            setter("explicit");
            return parent;
        }
    }
}