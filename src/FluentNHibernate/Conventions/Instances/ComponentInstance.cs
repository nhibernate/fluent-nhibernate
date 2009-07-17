using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class ComponentBaseInstance : IComponentBaseInstance
    {
        private readonly ComponentMappingBase mapping;
        protected readonly InspectorModelMapper<IComponentBaseInspector, ComponentMappingBase> propertyMappings = new InspectorModelMapper<IComponentBaseInspector, ComponentMappingBase>();

        public ComponentBaseInstance(ComponentMappingBase mapping)
        {
            this.mapping = mapping;
        }

        public IAccessInstance Access
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

        public virtual Type EntityType
        {
            get { return mapping.Type; }
        }
        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }
        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }
        public string ParentName
        {
            get { return mapping.Parent.Name; }
        }

        public bool Insert()
        {
            return mapping.Insert;
        }

        public bool Update()
        {
            return mapping.Update;
        }

        Access IAccessInspector.Access
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class ComponentInstance : ComponentBaseInstance, IComponentInstance
    {
        private ComponentMapping mapping;
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
    }

    public class DynamicComponentInstance : ComponentBaseInstance, IDynamicComponentInstance
    {
        private DynamicComponentMapping mapping;
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
    }
}
