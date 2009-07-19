using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class DiscriminatorInspector : IDiscriminatorInspector
    {
        private readonly InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping> propertyMappings = new InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping>();
        private readonly DiscriminatorMapping mapping;

        public DiscriminatorInspector(DiscriminatorMapping mapping)
        {
            this.mapping = mapping;
            propertyMappings.AutoMap();
        }

        public bool Insert
        {
            get { return mapping.Insert; }
        }

        public string Column
        {
            get { return mapping.Column; }
        }

        public bool Force
        {
            get { return mapping.Force; }
        }

        public string Formula
        {
            get { return mapping.Formula; }
        }

        public int Length
        {
            get { return mapping.Length; }
        }

        public bool NotNull
        {
            get { return mapping.NotNull; }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Type.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}