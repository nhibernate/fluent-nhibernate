using System;
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

        public IComponentInstance Not
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
