using System.Diagnostics;
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public new void Update()
        {
            if (mapping.IsSpecified("Update"))
                return;

            mapping.Update = nextBool;
            nextBool = true;
        }

        public new void Insert()
        {
            if (mapping.IsSpecified("Insert"))
                return;

            mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void Unique()
        {
            if (mapping.IsSpecified("Unique"))
                return;

            mapping.Unique = nextBool;
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            if (mapping.IsSpecified("OptimisticLock"))
                return;

            mapping.OptimisticLock = nextBool;
            nextBool = true;
        }
    }
}