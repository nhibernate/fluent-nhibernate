using System;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class ComponentInstance : ComponentInspector, IComponentInstance
    {
        private readonly ComponentMapping mapping;
        private bool nextBool;

        public ComponentInstance(ComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            nextBool = true;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IComponentInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new void LazyLoad()
        {
            if (mapping.IsSpecified("Lazy"))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
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
