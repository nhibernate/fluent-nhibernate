using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class EntityNameInstance : IEntityNameInstance
    {
        private readonly Action<string> setter;

		public EntityNameInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Name(string name)
        {
            setter(name);
        }
    }
}