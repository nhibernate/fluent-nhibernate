using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IIdentityMappingProvider
    {
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly IList<string> columns = new List<string>();
		private readonly PropertyInfo property;
        private readonly Type entityType;
        private readonly AccessStrategyBuilder<IdentityPart> access;
        private readonly AttributeStore<IdMapping> attributes = new AttributeStore<IdMapping>();

        public IdentityPart(Type entity, PropertyInfo property)
		{
            this.property = property;
            entityType = entity;

            access = new AccessStrategyBuilder<IdentityPart>(this, value => attributes.Set(x => x.Access, value));
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, property.PropertyType, entity);

            SetDefaultGenerator();
		}

        private void SetDefaultGenerator()
        {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType);

            if (property.PropertyType == typeof(Guid))
                defaultGenerator.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                defaultGenerator.Identity();
            else
                defaultGenerator.Assigned();

            attributes.SetDefault(x => x.Generator, generatorMapping);
        }

        IdMapping IIdentityMappingProvider.GetIdentityMapping()
        {
            var mapping = new IdMapping(attributes.CloneInner())
            {
                ContainingEntityType = entityType
            };

            if (columns.Count > 0)
            {
                foreach (var column in columns)
                    mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) {Name = column});
            }
            else
                mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = property.Name });

            mapping.Name = property.Name;
            mapping.SetDefaultValue(x => x.Type, new TypeReference(property.PropertyType));

            if (GeneratedBy.IsDirty)
                mapping.Generator = GeneratedBy.GetGeneratorMapping();

            return mapping;
        }

        public IdentityGenerationStrategyBuilder<IdentityPart> GeneratedBy { get; private set; }

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
        public AccessStrategyBuilder<IdentityPart> Access
	    {
	        get { return access; }
	    }

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        public IdentityPart UnsavedValue(object unsavedValue)
        {
            attributes.Set(x => x.UnsavedValue, unsavedValue.ToString());
            return this;
        }

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        public IdentityPart Column(string columnName)
        {
            columns.Clear(); // only currently support one column for ids
            columns.Add(columnName);
            return this;
        }

        public IdentityPart Length(int length)
        {
            attributes.Set(x => x.Length, length);
            return this;
        }
    }
}
