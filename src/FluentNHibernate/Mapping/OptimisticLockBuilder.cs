using System;

namespace FluentNHibernate.Mapping
{
    public class OptimisticLockBuilder<TParent> : IOptimisticLockBuilder
    {
        private readonly TParent parent;
        private readonly Action<string> setter;

        public OptimisticLockBuilder(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        /// <summary>
        /// Use no locking strategy
        /// </summary>
        public TParent None()
        {
            setter("none");
            return parent;
        }

        /// <summary>
        /// Use version locking
        /// </summary>
        public TParent Version()
        {
            setter("version");
            return parent;
        }

        /// <summary>
        /// Use dirty locking
        /// </summary>
        public TParent Dirty()
        {
            setter("dirty");
            return parent;
        }

        /// <summary>
        /// Use all locking
        /// </summary>
        public TParent All()
        {
            setter("all");
            return parent;
        }

        void IOptimisticLockBuilder.None()
        {
            None();
        }

        void IOptimisticLockBuilder.Version()
        {
            Version();
        }

        void IOptimisticLockBuilder.Dirty()
        {
            Dirty();
        }

        void IOptimisticLockBuilder.All()
        {
            All();
        }
    }
}