using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System.Diagnostics;

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
        private readonly Type identityType;
        private bool nextBool = true;
        private readonly string columnName;

        public IdentityPart(Type entity, PropertyInfo property)
        {
            this.property = property;
            entityType = entity;
            this.identityType = property.PropertyType;
            this.columnName = property.Name;

            access = new AccessStrategyBuilder<IdentityPart>(this, value => attributes.Set(x => x.Access, value));
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, property.PropertyType, entity);

            SetDefaultGenerator();
        }

        public IdentityPart(Type entity, Type identityType, string columnName)
        {
            this.property = null;
            this.entityType = entity;
            this.identityType = identityType;
            this.columnName = columnName;

            access = new AccessStrategyBuilder<IdentityPart>(this, value => attributes.Set(x => x.Access, value));
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, this.identityType, entity);

            SetDefaultGenerator();
        }

        private void SetDefaultGenerator()
        {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, identityType);

            if (identityType == typeof(Guid))
                defaultGenerator.GuidComb();
            else if (identityType == typeof(int) || identityType == typeof(long))
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
                    mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column });
            }
            else
                mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = columnName });

            if (property != null)
            {
                mapping.Name = columnName;
            }
            mapping.SetDefaultValue("Type", new TypeReference(identityType));

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IdentityPart Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        public IdentityPart UnsavedValue(object unsavedValue)
        {
            attributes.Set(x => x.UnsavedValue, (unsavedValue ?? "null").ToString());
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
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public IdentityPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        public IdentityPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        public IdentityPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public IdentityPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        public IdentityPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set(x => x.UniqueKey, keyColumns);
            return this;
        }

        public IdentityPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        public IdentityPart Index(string key)
        {
            columnAttributes.Set(x => x.Index, key);
            return this;
        }

        public IdentityPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        public IdentityPart Default(object value)
        {
            columnAttributes.Set(x => x.Default, value.ToString());
            return this;
        }

        public IdentityPart CustomType<T>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(T)));
            return this;
        }

        public IdentityPart CustomType(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        public IdentityPart CustomType(string type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }
    }
}