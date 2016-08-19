using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class DynamicComponentInspector : ComponentBaseInspector, IDynamicComponentInspector
    {
        private readonly InspectorModelMapper<IDynamicComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IDynamicComponentInspector, ComponentMapping>();
        private readonly IComponentMapping mapping;

        public DynamicComponentInspector(IComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public override bool IsSet(Member property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }
    }
}