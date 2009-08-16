using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class GeneratedInstance : IGeneratedInstance
    {
        private readonly Action<string> setter;

        public GeneratedInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Never()
        {
            setter("never");
        }

        public void Insert()
        {
            setter("insert");
        }

        public void Always()
        {
            setter("always");
        }
    }
}