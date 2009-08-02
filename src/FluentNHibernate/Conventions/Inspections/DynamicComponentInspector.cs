using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class DynamicComponentInspector : ComponentBaseInspector, IDynamicComponentInspector
    {
        private readonly InspectorModelMapper<IDynamicComponentInspector, DynamicComponentMapping> mappedProperties = new InspectorModelMapper<IDynamicComponentInspector, DynamicComponentMapping>();
        private readonly DynamicComponentMapping mapping;

        public DynamicComponentInspector(DynamicComponentMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
            mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
        }

        public override bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(mappedProperties.Get(property));
        }
    }
}