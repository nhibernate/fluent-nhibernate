using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class VersionInspector : IVersionInspector
    {
        private readonly InspectorModelMapper<IVersionInspector, VersionMapping> propertyMappings = new InspectorModelMapper<IVersionInspector, VersionMapping>();
        private readonly VersionMapping mapping;

        public VersionInspector(VersionMapping mapping)
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
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public string Column
        {
            get { return mapping.Column; }
        }

        public Generated Generated
        {
            get { return Generated.FromString(mapping.Generated); }
        }

        public string UnsavedValue
        {
            get { return mapping.UnsavedValue; }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }
    }
}