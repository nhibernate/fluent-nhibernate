using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class SchemaActionInstance : ISchemaActionInstance
    {
        private readonly Action<string> setter;

        public SchemaActionInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void None()
        {
            setter("none");
        }

        public void All()
        {
            setter("all");
        }

        public void Drop()
        {
            setter("drop");
        }

        public void Update()
        {
            setter("update");
        }

        public void Validate()
        {
            setter("validate");
        }

        public void Export()
        {
            setter("export");
        }

        public void Custom(string customValue)
        {
            setter(customValue);
        }
    }
}