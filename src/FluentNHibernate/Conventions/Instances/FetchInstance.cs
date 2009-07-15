using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class FetchInstance : IFetchInstance
    {
        private readonly Action<string> setter;

        public FetchInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Join()
        {
            setter("join");
        }

        public void Select()
        {
            setter("select");
        }
    }
}