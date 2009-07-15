using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class CascadeInstance : ICascadeInstance
    {
        private readonly Action<string> setter;

        public CascadeInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void All()
        {
            setter("all");
        }

        public void None()
        {
            setter("none");
        }

        public void SaveUpdate()
        {
            setter("save-update");
        }

        public void Delete()
        {
            setter("delete");
        }
    }
}