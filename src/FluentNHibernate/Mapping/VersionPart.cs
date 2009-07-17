using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IVersionMappingProvider
    {
        private readonly Type entity;
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly Cache<string, string> properties;
        private readonly VersionGeneratedBuilder<IVersionMappingProvider> generated;

        private readonly VersionMapping mapping = new VersionMapping();

        public VersionPart(Type entity, PropertyInfo property)
        {
            this.entity = entity;
            this.property = property;
            access = new AccessStrategyBuilder<VersionPart>(this, value => mapping.Access = value);
            generated = new VersionGeneratedBuilder<IVersionMappingProvider>(this, value => mapping.Generated = value);
            properties = new Cache<string, string>();
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            mapping.ContainingEntityType = entity;

            if (!mapping.IsSpecified(x => x.Name))
                mapping.SetDefaultValue(x => x.Name, property.Name);

            if (!mapping.IsSpecified(x => x.Type))
                mapping.SetDefaultValue(x => x.Type, property.PropertyType == typeof(DateTime) ? new TypeReference("timestamp") : new TypeReference(property.PropertyType));

            if (!mapping.IsSpecified(x => x.Column))
                mapping.SetDefaultValue(x => x.Column, property.Name);

            return mapping;
        }

        public VersionGeneratedBuilder<IVersionMappingProvider> Generated
        {
            get { return generated; }
        }

        public AccessStrategyBuilder<VersionPart> Access
        {
            get { return access; }
        }

        public IVersionMappingProvider ColumnName(string name)
        {
            mapping.Column = name;
            return this;
        }

        public IVersionMappingProvider UnsavedValue(string value)
        {
            mapping.UnsavedValue = value;
            return this;
        }
    }
}