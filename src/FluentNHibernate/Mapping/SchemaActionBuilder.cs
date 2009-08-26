using System;

namespace FluentNHibernate.Mapping
{
    public class SchemaActionBuilder<T>
    {
        private readonly T parent;
        private readonly Action<string> setter;

        public SchemaActionBuilder(T parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public T All()
        {
            setter("all");
            return parent;
        }

        public T None()
        {
            setter("none");
            return parent;
        }

        public T Drop()
        {
            setter("drop");
            return parent;
        }

        public T Update()
        {
            setter("update");
            return parent;
        }

        public T Export()
        {
            setter("export");
            return parent;
        }

        public T Validate()
        {
            setter("validate");
            return parent;
        }

        public T Custom(string customValue)
        {
            setter(customValue);
            return parent;
        }
    }
}