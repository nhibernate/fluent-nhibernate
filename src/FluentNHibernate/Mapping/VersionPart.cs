using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IVersionMappingProvider
    {
        private readonly Type entity;
        private readonly Member property;
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly VersionGeneratedBuilder<IVersionMappingProvider> generated;
        private readonly AttributeStore<VersionMapping> attributes = new AttributeStore<VersionMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly List<string> columns = new List<string>();
        private bool nextBool = true;

        public VersionPart(Type entity, Member property)
        {
            this.entity = entity;
            this.property = property;
            access = new AccessStrategyBuilder<VersionPart>(this, value => attributes.Set(x => x.Access, value));
            generated = new VersionGeneratedBuilder<IVersionMappingProvider>(this, value => attributes.Set(x => x.Generated, value));
        }

        /// <summary>
        /// Specify if this version is database generated
        /// </summary>
        public VersionGeneratedBuilder<IVersionMappingProvider> Generated
        {
            get { return generated; }
        }

        /// <summary>
        /// Specify the access strategy
        /// </summary>
        public AccessStrategyBuilder<VersionPart> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public VersionPart Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Specify the column name
        /// </summary>
        /// <param name="name">Column name</param>
        public VersionPart Column(string name)
        {
            columns.Add(name);
            return this;
        }

        /// <summary>
        /// Specify the unsaved value
        /// </summary>
        /// <param name="value">Unsaved value</param>
        public VersionPart UnsavedValue(string value)
        {
            attributes.Set(x => x.UnsavedValue, value);
            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public VersionPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Specify decimal precision
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public VersionPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        /// <summary>
        /// Specify decimal scale
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public VersionPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        /// <summary>
        /// Specify the nullability of the column
        /// </summary>
        public VersionPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the uniqueness of the column
        /// </summary>
        public VersionPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify a unique key constraint
        /// </summary>
        /// <param name="keyColumns">Constraint columns</param>
        public VersionPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set(x => x.UniqueKey, keyColumns);
            return this;
        }

        /// <summary>
        /// Specify an index name
        /// </summary>
        /// <param name="index">Index name</param>
        public VersionPart Index(string index)
        {
            columnAttributes.Set(x => x.Index, index);
            return this;
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public VersionPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        /// <summary>
        /// Specify a default value
        /// </summary>
        /// <param name="value">Default value</param>
        public VersionPart Default(object value)
        {
            columnAttributes.Set(x => x.Default, value.ToString());
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <typeparam name="T">Custom type</typeparam>
        public VersionPart CustomType<T>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(T)));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <param name="type">Custom type</param>
        public VersionPart CustomType(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <param name="type">Custom type</param>
        public VersionPart CustomType(string type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specify a custom SQL type
        /// </summary>
        /// <param name="sqlType">SQL type</param>
        public VersionPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            var mapping = new VersionMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;

            mapping.SetDefaultValue("Name", property.Name);
            mapping.SetDefaultValue("Type", property.PropertyType == typeof(DateTime) ? new TypeReference("timestamp") : new TypeReference(property.PropertyType));
            mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = property.Name });

            columns.ForEach(column => mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column }));

            return mapping;
        }
    }
}