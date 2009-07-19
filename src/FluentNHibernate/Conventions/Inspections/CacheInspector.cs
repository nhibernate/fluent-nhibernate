using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CacheInspector : ICacheInspector
    {
        private readonly InspectorModelMapper<ICacheInspector, CacheMapping> propertyMappings = new InspectorModelMapper<ICacheInspector, CacheMapping>();
        private readonly CacheMapping mapping;

        public CacheInspector(CacheMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.AutoMap();
        }

        public string Usage
        {
            get { return mapping.Usage; }
        }

        public string Region
        {
            get { return mapping.Region; }
        }

        public Type EntityType
        {
            get { return mapping.ContainedEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Usage; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}