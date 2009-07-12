using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IIdentityPart
    {
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly IList<string> columns = new List<string>();
		private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<IIdentityPart> access;

        private readonly IdMapping mapping;

        public IdentityPart(Type entity, PropertyInfo property, string columnName)
		{
            this.property = property;
            EntityType = entity;

            mapping = new IdMapping { ContainingEntityType = EntityType };
            access = new AccessStrategyBuilder<IIdentityPart>(this, value => mapping.Access = value);
            GeneratedBy = new IdentityGenerationStrategyBuilder<IIdentityPart>(this, IdentityType);

            ColumnName(columnName);

            SetDefaultGenerator();
		}

        public IdentityPart(Type entity, PropertyInfo property)
            : this(entity, property, property.Name)
		{}

        private void SetDefaultGenerator()
        {
            if (IdentityType == typeof(Guid))
                GeneratedBy.GuidComb();
            else if (IdentityType == typeof(int) || IdentityType == typeof(long))
                GeneratedBy.Identity();
            else
                GeneratedBy.Assigned();
        }

        IdMapping IIdentityPart.GetIdMapping()
        {
            foreach (var column in columns)
                mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column });

            mapping.Name = Property.Name;
            mapping.Type = new TypeReference(Property.PropertyType);
            mapping.Generator = GeneratedBy.GetGeneratorMapping();

            return mapping;
        }

        public IdentityGenerationStrategyBuilder<IIdentityPart> GeneratedBy { get; private set; }

        public Type IdentityType
		{
			get { return property.PropertyType; }
		}

        public Type EntityType { get; private set; }

        public PropertyInfo Property
        {
            get { return property; }
        }

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
        public AccessStrategyBuilder<IIdentityPart> Access
	    {
	        get { return access; }
	    }

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        public IIdentityPart UnsavedValue(object unsavedValue)
        {
            mapping.UnsavedValue = unsavedValue.ToString();
            return this;
        }

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        public IIdentityPart ColumnName(string columnName)
        {
            columns.Clear(); // only currently support one column for ids
            columns.Add(columnName);
            return this;
        }

        /// <summary>
        /// Gets the column name
        /// </summary>
        /// <returns></returns>
        public string GetColumnName()
        {
            var column = mapping.Columns.FirstOrDefault();

            return column != null ? column.Name : null;
        }
    }
}
