using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public abstract class AccessConvention
        : IIdConvention,
          IVersionConvention,
          IPropertyConvention,
          IComponentConvention,
          IReferenceConvention,
          IHasOneConvention,
          ICollectionConvention
    {
        protected abstract void Apply(Type owner, string name, IAccessInstance access);

        public virtual void Apply(IIdentityInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(IVersionInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(IPropertyInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(IComponentInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(IOneToOneInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(IManyToOneInstance instance)
        {
            Apply(instance.EntityType, instance.Name, instance.Access);
        }

        public virtual void Apply(ICollectionInstance instance)
        {
            string name = ((ICollectionInspector)instance).Name;
            Apply(instance.EntityType, name, instance.Access);
        }
    }
}
