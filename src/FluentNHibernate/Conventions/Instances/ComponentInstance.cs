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

        public Access Access
        {
            get { throw new NotImplementedException(); }
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
        public bool Insert
        {
            get { return mapping.Insert; }
        }
        public bool Update
        {
            get { return mapping.Update; }
        }
    }

    public class ComponentInstance : ComponentBaseInstance, IComponentInstance
    {
        private ComponentMapping mapping;

        public ComponentInstance(ComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }
    }

    public class DynamicComponentInstance : ComponentBaseInstance, IDynamicComponentInstance
    {
        private DynamicComponentMapping mapping;
        public DynamicComponentInstance(DynamicComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }
    }
}
