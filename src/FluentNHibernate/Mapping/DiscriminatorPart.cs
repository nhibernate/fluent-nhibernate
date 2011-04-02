using System;
using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart : IDiscriminatorMappingProvider
    {
        private readonly string columnName;
        private readonly Type entity;
        private readonly Action<Type, ISubclassMappingProvider> setter;
        private readonly TypeReference discriminatorValueType;
        private readonly AttributeStore<DiscriminatorMapping> attributes = new AttributeStore<DiscriminatorMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private bool nextBool = true;

        public DiscriminatorPart(string columnName, Type entity, Action<Type, ISubclassMappingProvider> setter, TypeReference discriminatorValueType)
        {
            this.columnName = columnName;
            this.entity = entity;
            this.setter = setter;
            this.discriminatorValueType = discriminatorValueType;
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public DiscriminatorPart SubClass<TSubClass>(object discriminatorValue, Action<SubClassPart<TSubClass>> action)
        {
            var subclass = new SubClassPart<TSubClass>(this, discriminatorValue);

            action(subclass);
            setter(typeof(TSubClass), subclass);

            return this;
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public DiscriminatorPart SubClass<TSubClass>(Action<SubClassPart<TSubClass>> action)
        {
            return SubClass(null, action);
        }

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DiscriminatorPart Not
        {
             get
             {
                 nextBool = !nextBool;
                 return this;
             }
        }

        /// <summary>
        /// Force NHibernate to always select using the discriminator value, even when selecting all subclasses. This
        /// can be useful when your table contains more discriminator values than you have classes (legacy).
        /// </summary>
        /// <remarks>Sets the "force" attribute.</remarks>
        public DiscriminatorPart AlwaysSelectWithValue()
        {
            attributes.Set(x => x.Force, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Set this discriminator as read-only. Call this if your discriminator column is also part of a mapped composite identifier.
        /// </summary>
        /// <returns>Sets the "insert" attribute.</returns>
        public DiscriminatorPart ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// An arbitrary SQL expression that is executed when a type has to be evaluated. Allows content-based discrimination.
        /// </summary>
        /// <param name="sql">SQL expression</param>
        public DiscriminatorPart Formula(string sql)
        {
            attributes.Set(x => x.Formula, sql);
            return this;
        }

        /// <summary>
        /// Sets the precision for decimals
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public DiscriminatorPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        /// <summary>
        /// Specifies the scale for decimals
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public DiscriminatorPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public DiscriminatorPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Specify the nullability of this column
        /// </summary>
        public DiscriminatorPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the uniqueness of this column
        /// </summary>
        public DiscriminatorPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the unique key constraint name
        /// </summary>
        /// <param name="keyColumns">Constraint columns</param>
        public DiscriminatorPart UniqueKey(string keyColumns)
        {
            columnAttributes.Set(x => x.UniqueKey, keyColumns);
            return this;
        }

        /// <summary>
        /// Specifies the index name
        /// </summary>
        /// <param name="index">Index name</param>
        public DiscriminatorPart Index(string index)
        {
            columnAttributes.Set(x => x.Index, index);
            return this;
        }

        /// <summary>
        /// Specifies a check constraint name
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public DiscriminatorPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        /// <summary>
        /// Specifies the default value for the discriminator
        /// </summary>
        /// <param name="value">Default value</param>
        public DiscriminatorPart Default(object value)
        {
            columnAttributes.Set(x => x.Default, value.ToString());
            return this;
        }

        /// <summary>
        /// Specifies a custom type for the discriminator
        /// </summary>
        /// <remarks>
        /// This is often used with <see cref="IUserType"/>
        /// </remarks>
        /// <typeparam name="T">Custom type</typeparam>
        public DiscriminatorPart CustomType<T>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(T)));
            return this;
        }

        /// <summary>
        /// Specifies a custom type for the discriminator
        /// </summary>
        /// <remarks>
        /// This is often used with <see cref="IUserType"/>
        /// </remarks>
        /// <param name="type">Custom type</param>
        public DiscriminatorPart CustomType(Type type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specifies a custom type for the discriminator
        /// </summary>
        /// <remarks>
        /// This is often used with <see cref="IUserType"/>
        /// </remarks>
        /// <param name="type">Custom type</param>
        public DiscriminatorPart CustomType(string type)
        {
            attributes.Set(x => x.Type, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Specifies a custom SQL type for the discriminator.
        /// </summary>
        /// <param name="type">Custom SQL type.</param>
        public DiscriminatorPart SqlType(string type) {
            columnAttributes.Set(x => x.SqlType, type);
            return this;
        }

        DiscriminatorMapping IDiscriminatorMappingProvider.GetDiscriminatorMapping()
        {
            var mapping = new DiscriminatorMapping(attributes.CloneInner())
            {
                ContainingEntityType = entity,
            };

            mapping.SetDefaultValue("Type", discriminatorValueType);

            mapping.AddColumn(new ColumnMapping(columnAttributes.CloneInner()) { Name = columnName });

            return mapping;
        }
    }
}