using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            if (!mapping.IsSpecified("Lazy"))
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
            if (!mapping.IsSpecified("Update"))
                mapping.Update = nextBool;
            nextBool = true;
        }

        public new void Insert()
        {
            if (!mapping.IsSpecified("Insert"))
                mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void Unique()
        {
            if (!mapping.IsSpecified("Unique"))
                mapping.Unique = nextBool;
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            if (!mapping.IsSpecified("OptimisticLock"))
                mapping.OptimisticLock = nextBool;
            nextBool = true;
        }

        public new IEnumerable<IOneToOneInstance> OneToOnes
        {
            get { return mapping.OneToOnes.Select(x => new OneToOneInstance(x)).Cast<IOneToOneInstance>(); }
        }
        public new IEnumerable<IPropertyInstance> Properties
        {
            get { return mapping.Properties.Select(x => new PropertyInstance(x)).Cast<IPropertyInstance>(); }
        }        
    }
}
