using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class OptimisticLockInstance : IOptimisticLockInstance
    {
        private readonly Action<string> setter;

        public OptimisticLockInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void None()
        {
            setter("none");
        }

        public void Version()
        {
            setter("version");
        }

        public void Dirty()
        {
            setter("dirty");
        }

        public void All()
        {
            setter("all");
        }
    }
}