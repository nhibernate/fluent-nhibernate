using System;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IVersionMappingProvider
    {
        private readonly Type entity;
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly VersionGeneratedBuilder<IVersionMappingProvider> generated;
        private readonly AttributeStore<VersionMapping> attributes = new AttributeStore<VersionMapping>();

        public VersionPart(Type entity, PropertyInfo property)
        {
            this.entity = entity;
            this.property = property;
            access = new AccessStrategyBuilder<VersionPart>(this, value => attributes.Set(x => x.Access, value));
            generated = new VersionGeneratedBuilder<IVersionMappingProvider>(this, value => attributes.Set(x => x.Generated, value));
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            var mapping = new VersionMapping(attributes.CloneInner());

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

        public VersionPart Column(string name)
        {
            attributes.Set(x => x.Column, name);
            return this;
        }

        public VersionPart UnsavedValue(string value)
        {
            attributes.Set(x => x.UnsavedValue, value);
            return this;
        }
    }
}