using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class DynamicComponentInstance : DynamicComponentInspector, IDynamicComponentInstance
    {
        private readonly DynamicComponentMapping mapping;
        private bool nextBool;

        public DynamicComponentInstance(DynamicComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            nextBool = true;
        }

        public IDynamicComponentInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }


        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Access))
                        mapping.Access = value;
                });
            }
        }

        public new void Update()
        {
            if (mapping.IsSpecified(x => x.Update))
                return;

            mapping.Update = nextBool;
            nextBool = true;
        }

        public new void Insert()
        {
            if (mapping.IsSpecified(x => x.Insert))
                return;

            mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void Unique()
        {
            if (mapping.IsSpecified(x => x.Unique))
                return;

            mapping.Unique = nextBool;
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            if (mapping.IsSpecified(x => x.OptimisticLock))
                return;

            mapping.OptimisticLock = nextBool;
            nextBool = true;
        }
    }
}