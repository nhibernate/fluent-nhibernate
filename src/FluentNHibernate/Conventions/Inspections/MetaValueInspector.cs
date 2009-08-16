using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class MetaValueInspector : IMetaValueInspector
    {
        private readonly InspectorModelMapper<IMetaValueInspector, MetaValueMapping> propertyMappings = new InspectorModelMapper<IMetaValueInspector, MetaValueMapping>();
        private readonly MetaValueMapping mapping;

        public MetaValueInspector(MetaValueMapping mapping)
        {
            this.mapping = mapping;
            propertyMappings.AutoMap();
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Class.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public string Value
        {
            get { return mapping.Value; }
        }
    }
}