using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ParentInspector : IParentInspector
    {
        private readonly InspectorModelMapper<IPropertyInspector, ParentMapping> mappedProperties = new InspectorModelMapper<IPropertyInspector, ParentMapping>();
        private readonly ParentMapping mapping;

        public ParentInspector(ParentMapping mapping)
        {
            this.mapping = mapping;
            mappedProperties.AutoMap();
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
            return mapping.IsSpecified(mappedProperties.Get(property));
        }

        public string Name
        {
            get { return mapping.Name; }
        }
    }
}