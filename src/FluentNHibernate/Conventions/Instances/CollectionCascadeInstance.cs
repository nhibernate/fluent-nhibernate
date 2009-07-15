using System;

namespace FluentNHibernate.Conventions.Instances
{
    public class CollectionCascadeInstance : CascadeInstance, ICollectionCascadeInstance
    {
        private readonly Action<string> setter;

        public CollectionCascadeInstance(Action<string> setter)
            : base(setter)
        {
            this.setter = setter;
        }

        public void AllDeleteOrphan()
        {
            setter("all-delete-orphan");
        }
    }
}