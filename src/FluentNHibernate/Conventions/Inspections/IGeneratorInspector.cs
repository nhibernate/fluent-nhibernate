using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IGeneratorInspector : IInspector
    {
        string Class { get; }
        IDictionary<string, string> Params { get; }
    }

    public class GeneratorInspector : IGeneratorInspector
    {
        private readonly InspectorModelMapper<IGeneratorInspector, GeneratorMapping> propertyMappings = new InspectorModelMapper<IGeneratorInspector, GeneratorMapping>();
        private readonly GeneratorMapping mapping;

        public GeneratorInspector(GeneratorMapping mapping)
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
            get { return mapping.Class; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public string Class
        {
            get { return mapping.Class; }
        }

        public IDictionary<string, string> Params
        {
            get { return new Dictionary<string, string>(mapping.Params); }
        }
    }
}