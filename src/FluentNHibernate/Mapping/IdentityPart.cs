using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private readonly IdMapping mapping;

        public IdentityPart(Type entity, PropertyInfo property, string columnName)
		{
            this.property = property;
            entityType = entity;

            mapping = new IdMapping { ContainingEntityType = entityType };
            access = new AccessStrategyBuilder<IdentityPart>(this, value => mapping.Access = value);
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, property.PropertyType, entity);

            ColumnName(columnName);

            SetDefaultGenerator();
		}

        public IdentityPart(Type entity, PropertyInfo property)
            : this(entity, property, property.Name)
		{}

        private void SetDefaultGenerator()
        {
            if (property.PropertyType == typeof(Guid))
                GeneratedBy.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                GeneratedBy.Identity();
            else
                GeneratedBy.Assigned();
        }

        IdMapping IIdentityMappingProvider.GetIdentityMapping()
        {
            foreach (var column in columns)
                mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column });

            mapping.Name = property.Name;
            mapping.Type = new TypeReference(property.PropertyType);
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
            mapping.UnsavedValue = unsavedValue.ToString();
            return this;
        }

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        public IdentityPart ColumnName(string columnName)
        {
            columns.Clear(); // only currently support one column for ids
            columns.Add(columnName);
            return this;
        }
    }
}
