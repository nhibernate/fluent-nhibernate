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
        private readonly Member member;
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly VersionGeneratedBuilder<VersionPart> generated;
        private readonly AttributeStore<VersionMapping> attributes = new AttributeStore<VersionMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly List<string> columns = new List<string>();
        private bool nextBool = true;

        public VersionPart(Type entity, Member member)
        {
            this.entity = entity;
            this.member = member;
            access = new AccessStrategyBuilder<VersionPart>(this, value => attributes.Set(x => x.Access, value));
            generated = new VersionGeneratedBuilder<VersionPart>(this, value => attributes.Set(x => x.Generated, value));

            SetDefaultAccess();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.SetDefault(x => x.Access, resolvedAccess.ToString());
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

            mapping.SetDefaultValue("Name", member.Name);
            mapping.SetDefaultValue("Type", member.PropertyType == typeof(DateTime) ? new TypeReference("timestamp") : new TypeReference(member.PropertyType));
            mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = member.Name });

            columns.ForEach(column => mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = column }));

            return mapping;
        }
    }
}