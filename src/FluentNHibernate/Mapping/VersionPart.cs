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
        readonly Type entity;
        readonly Member member;
        readonly AccessStrategyBuilder<VersionPart> access;
        readonly VersionGeneratedBuilder<VersionPart> generated;
        readonly AttributeStore attributes = new AttributeStore();
        readonly AttributeStore columnAttributes = new AttributeStore();
        readonly List<string> columns = new List<string>();
        bool nextBool = true;

        public VersionPart(Type entity, Member member)
        {
            this.entity = entity;
            this.member = member;
            access = new AccessStrategyBuilder<VersionPart>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            generated = new VersionGeneratedBuilder<VersionPart>(this, value => attributes.Set("Generated", Layer.UserSupplied, value));

            SetDefaultAccess();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.Set("Access", Layer.Defaults, resolvedAccess.ToString());
        }

        /// <summary>
        /// Specify if this version is database generated
        /// </summary>
        public VersionGeneratedBuilder<VersionPart> Generated
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
            attributes.Set("UnsavedValue", Layer.UserSupplied, value);
            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public VersionPart Length(int length)
        {
            columnAttributes.Set("Length", Layer.UserSupplied, length);
            return this;
        }

        /// <summary>
        /// Specify decimal precision
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public VersionPart Precision(int precision)
        {
            columnAttributes.Set("Precision", Layer.UserSupplied, precision);
            return this;
        }

        /// <summary>
        /// Specify decimal scale
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public VersionPart Scale(int scale)
        {
            columnAttributes.Set("Scale", Layer.UserSupplied, scale);
            return this;
        }

        /// <summary>
        /// Specify the nullability of the column
        /// </summary>
        public VersionPart Nullable()
        {
            columnAttributes.Set("NotNull", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the uniqueness of the column
        /// </summary>
        public VersionPart Unique()
        {
            columnAttributes.Set("Unique", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify a unique key constraint
        /// </summary>
        /// <param name="keyColumns">Constraint columns</param>
        public VersionPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set("UniqueKey", Layer.UserSupplied, keyColumns);
            return this;
        }

        /// <summary>
        /// Specify an index name
        /// </summary>
        /// <param name="index">Index name</param>
        public VersionPart Index(string index)
        {
            columnAttributes.Set("Index", Layer.UserSupplied, index);
            return this;
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public VersionPart Check(string constraint)
        {
            columnAttributes.Set("Check", Layer.UserSupplied, constraint);
            return this;
        }

        /// <summary>
        /// Specify a default value
        /// </summary>
        /// <param name="value">Default value</param>
        public VersionPart Default(object value)
        {
            columnAttributes.Set("Default", Layer.UserSupplied, value.ToString());
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <typeparam name="T">Custom type</typeparam>
        public VersionPart CustomType<T>()
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(typeof(T)));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <param name="type">Custom type</param>
        public VersionPart CustomType(Type type)
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>Usually used with an <see cref="IUserType"/></remarks>
        /// <param name="type">Custom type</param>
        public VersionPart CustomType(string type)
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specify a custom SQL type
        /// </summary>
        /// <param name="sqlType">SQL type</param>
        public VersionPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set("SqlType", Layer.UserSupplied, sqlType);
            return this;
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            var mapping = new VersionMapping(attributes.Clone())
            {
                ContainingEntityType = entity
            };

            mapping.Set(x => x.Name, Layer.Defaults, member.Name);
            mapping.Set(x => x.Type, Layer.Defaults, member.PropertyType == typeof(DateTime) ? new TypeReference("timestamp") : new TypeReference(member.PropertyType));

            var defaultColumnMapping = new ColumnMapping(columnAttributes.Clone());
            defaultColumnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
            mapping.AddColumn(Layer.Defaults, defaultColumnMapping);

            columns.ForEach(column =>
            {
                var columnMapping = new ColumnMapping(columnAttributes.Clone());
                columnMapping.Set(x => x.Name, Layer.Defaults, column);
                mapping.AddColumn(Layer.UserSupplied, columnMapping);
            });

            return mapping;
        }
    }
}