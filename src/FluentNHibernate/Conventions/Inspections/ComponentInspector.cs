using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ComponentInspector : ComponentBaseInspector, IComponentInspector
    {
        private readonly InspectorModelMapper<IComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IComponentInspector, ComponentMapping>();
        private readonly ComponentMapping mapping;

        public ComponentInspector(ComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public override bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }
    }
}