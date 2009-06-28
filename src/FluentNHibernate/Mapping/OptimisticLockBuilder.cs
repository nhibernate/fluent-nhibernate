using System;

namespace FluentNHibernate.Mapping
{
    public class OptimisticLockBuilder : IOptimisticLockBuilder
    {
        private readonly Action<string> setter;

        public OptimisticLockBuilder(Action<string> setter)
        {
            this.setter = setter;
        }

        void IOptimisticLockBuilder.None()
        {
            setter("none");
        }

        void IOptimisticLockBuilder.Version()
        {
            setter("version");
        }

        void IOptimisticLockBuilder.Dirty()
        {
            setter("dirty");
        }

        void IOptimisticLockBuilder.All()
        {
            setter("all");
        }
    }

    public class OptimisticLockBuilder<TParent> : OptimisticLockBuilder
    {
        private readonly TParent parent;

        public OptimisticLockBuilder(TParent parent, Action<string> setter)
            : base(setter)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Use no locking strategy
        /// </summary>
        public TParent None()
        {
            ((IOptimisticLockBuilder)this).None();
            return parent;
        }

        /// <summary>
        /// Use version locking
        /// </summary>
        public TParent Version()
        {
            ((IOptimisticLockBuilder)this).Version();
            return parent;
        }

        /// <summary>
        /// Use dirty locking
        /// </summary>
        public TParent Dirty()
        {
            ((IOptimisticLockBuilder)this).Dirty();
            return parent;
        }

        /// <summary>
        /// Use all locking
        /// </summary>
        public TParent All()
        {
            ((IOptimisticLockBuilder)this).All();
            return parent;
        }
    }
}