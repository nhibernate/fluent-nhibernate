using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class DynamicComponentInstance : DynamicComponentInspector, IDynamicComponentInstance
    {
        private readonly ComponentMapping mapping;
        private bool nextBool;

        public DynamicComponentInstance(ComponentMapping mapping)
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
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new void Update()
        {
            mapping.Set(x => x.Update, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Insert()
        {
            mapping.Set(x => x.Insert, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Unique()
        {
            mapping.Set(x => x.Unique, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            mapping.Set(x => x.OptimisticLock, Layer.Conventions, nextBool);
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