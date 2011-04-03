using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System.Diagnostics;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IIdentityMappingProvider
    {
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly IList<string> columns = new List<string>();
        private Member member;
        private readonly Type entityType;
        private readonly AccessStrategyBuilder<IdentityPart> access;
        private readonly AttributeStore<IdMapping> attributes = new AttributeStore<IdMapping>();
        private Type identityType;
        private bool nextBool = true;
        string name;

        public IdentityPart(Type entity, Member member)
        {
            entityType = entity;
            this.member = member;
            identityType = member.PropertyType;
            
            access = new AccessStrategyBuilder<IdentityPart>(this, value => attributes.Set(x => x.Access, value));
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, member.PropertyType, entityType);
            SetName(member.Name);
            SetDefaultGenerator();
            SetDefaultAccess();
        }

        public IdentityPart(Type entity, Type identityType)
        {
            this.entityType = entity;
            this.identityType = identityType;

            access = new AccessStrategyBuilder<IdentityPart>(this, value => attributes.Set(x => x.Access, value));
            GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, this.identityType, entity);

            SetDefaultGenerator();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.SetDefault(x => x.Access, resolvedAccess.ToString());
        }

        /// <summary>
        /// Specify the generator
        /// </summary>
        /// <example>
        /// Id(x => x.PersonId)
        ///   .GeneratedBy.Assigned();
        /// </example>
        public IdentityGenerationStrategyBuilder<IdentityPart> GeneratedBy { get; private set; }

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
        public AccessStrategyBuilder<IdentityPart> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
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

        /// <summary>
        /// Specify the identity column length
        /// </summary>
        /// <param name="length">Column length</param>
        public IdentityPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Specify the decimal precision
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public IdentityPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        /// <summary>
        /// Specify the decimal scale
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public IdentityPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        /// <summary>
        /// Specify the nullability of the identity column
        /// </summary>
        public IdentityPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the uniqueness of the identity column
        /// </summary>
        public IdentityPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify a unique key constraint
        /// </summary>
        /// <param name="keyColumns">Constraint columns</param>
        public IdentityPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set(x => x.UniqueKey, keyColumns);
            return this;
        }

        /// <summary>
        /// Specify a custom SQL type
        /// </summary>
        /// <param name="sqlType">SQL type</param>
        public IdentityPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        /// <summary>
        /// Specify an index name
        /// </summary>
        /// <param name="key">Index name</param>
        public IdentityPart Index(string key)
        {
            columnAttributes.Set(x => x.Index, key);
            return this;
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public IdentityPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        /// <summary>
        /// Specify a default value
        /// </summary>
        /// <param name="value">Default value</param>
        public IdentityPart Default(object value)
        {
            columnAttributes.Set(x => x.Default, value.ToString());
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>
        /// This is usually used with an <see cref="IUserType"/>
        /// </remarks>
        /// <typeparam name="T">Custom type</typeparam>
        public IdentityPart CustomType<T>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(T)));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>
        /// This is usually used with an <see cref="IUserType"/>
        /// </remarks>
        /// <param name="type">Custom type</param>
        public IdentityPart CustomType(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specify a custom type
        /// </summary>
        /// <remarks>
        /// This is usually used with an <see cref="IUserType"/>
        /// </remarks>
        /// <param name="type">Custom type</param>
        public IdentityPart CustomType(string type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        internal void SetName(string newName)
        {
            name = newName;
        }

        bool HasNameSpecified
        {
            get { return !string.IsNullOrEmpty(name); }
        }

        void SetDefaultGenerator()
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
            else if (HasNameSpecified)
                mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = name });

            if (member != null)
            {
                mapping.Name = name;
            }
            mapping.SetDefaultValue("Type", new TypeReference(identityType));

            if (GeneratedBy.IsDirty)
                mapping.Generator = GeneratedBy.GetGeneratorMapping();

            return mapping;
        }
    }
}