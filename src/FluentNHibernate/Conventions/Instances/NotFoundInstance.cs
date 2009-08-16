using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class NotFoundInstance : INotFoundInstance
    {
        private readonly Action<string> setter;

        public NotFoundInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Ignore()
        {
            setter("ignore");
        }

        public void Exception()
        {
            setter("exception");
        }
    }
}