using System;

namespace FluentNHibernate.Mapping
{
    public class CascadeExpression<TParent>
    {
        private readonly TParent parent;
        private readonly Action<string> setter;

        public CascadeExpression(TParent parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        /// <summary>
        /// Cascade all actions
        /// </summary>
        public TParent All()
        {
            setter("all");
            return parent;
        }

        /// <summary>
        /// Cascade no actions
        /// </summary>
        public TParent None()
        {
            setter("none");
            return parent;
        }

        /// <summary>
        /// Cascade saves and updates
        /// </summary>
        public TParent SaveUpdate()
        {
            setter("save-update");
            return parent;
        }

        /// <summary>
        /// Cascade deletes
        /// </summary>
        public TParent Delete()
        {
            setter("delete");
            return parent;
        }

        /// <summary>
        /// Cascade all actions, deleting any orphaned records
        /// </summary>
        public TParent AllDeleteOrphan()
        {
            setter("all-delete-orphan");
            return parent;
        }

        /// <summary>
        /// Cascade merges
        /// </summary>
        public TParent Merge()
        {
            setter("merge");
            return parent;
        }

        /// <summary>
        /// Cascade on replication
        /// </summary>
        public TParent Replicate()
        {
            setter("replicate");
            return parent;
        }

        /// <summary>
        /// Cascade refreshes
        /// </summary>
        public TParent Refresh()
        {
            setter("refresh");
            return parent;
        }

        /// <summary>
        /// Cascade deletes orphans
        /// </summary>
        public TParent DeleteOrphans()
        {
            setter("delete-orphans");
            return parent;
        }

        /// <summary>
        /// Cascade evicts
        /// </summary>
        public TParent Evict()
        {
            setter("evict");
            return parent;
        }

        /// <summary>
        /// Cascade locks
        /// </summary>
        public TParent Lock()
        {
            setter("lock");
            return parent;
        }

        /// <summary>
        /// Cascade persists
        /// </summary>
        public TParent Persist()
        {
            setter("save-update, persist");
            return parent;
        }
    }
}