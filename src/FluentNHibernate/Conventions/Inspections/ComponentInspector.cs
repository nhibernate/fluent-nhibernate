using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ComponentInspector : ComponentBaseInspector, IComponentInspector
    {
        private readonly InspectorModelMapper<IComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IComponentInspector, ComponentMapping>();
        private readonly IComponentMapping mapping;

        public ComponentInspector(IComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public override bool IsSet(Member property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }
    }
}