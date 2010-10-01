using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping.Builders
{
    public class PropertyBuilder
    {
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();

        private bool nextBool = true;
        readonly PropertyMapping mapping;

        public PropertyBuilder(PropertyMapping mapping, Type containingEntityType, Member member)
        {
            this.mapping = mapping;

            InitialiseDefaults(containingEntityType, member);
        }

        void InitialiseDefaults(Type containingEntityType, Member member)
        {
            mapping.ContainingEntityType = containingEntityType;
            mapping.Member = member;
            mapping.AddDefaultColumn(new ColumnMapping(columnAttributes.InnerStore) { Name = member.Name });
            mapping.SetDefaultValue("Name", mapping.Member.Name);
            mapping.SetDefaultValue("Type", GetDefaultType());

            if (member.PropertyType.IsEnum() && member.PropertyType.IsNullable())
                columnAttributes.SetDefault(x => x.NotNull, false);
        }

        /// <summary>
        /// Specify if this property is database generated
        /// </summary>
        /// <example>
        /// Generated.Insert();
        /// </example>
        public PropertyGeneratedBuilder Generated
        {
            get { return new PropertyGeneratedBuilder(this, value => mapping.Generated = value); }
        }

        /// <summary>
        /// Specify the property column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public PropertyBuilder Column(string columnName)
        {
            Columns.Clear();
            Columns.Add(columnName);
            return this;
        }

        /// <summary>
        /// Modify the columns collection
        /// </summary>
        public ColumnMappingCollection<PropertyBuilder> Columns
        {
            get { return new ColumnMappingCollection<PropertyBuilder>(this, mapping, columnAttributes.InnerStore); }
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyBuilder> Access
        {
            get { return new AccessStrategyBuilder<PropertyBuilder>(this, value => mapping.Access = value); }
        }

        /// <summary>
        /// Specify that this property is insertable
        /// </summary>
        public PropertyBuilder Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;

            return this;
        }

        /// <summary>
        /// Specify that this property is updatable
        /// </summary>
        public PropertyBuilder Update()
        {
            mapping.Update = nextBool;
            nextBool = true;

            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public PropertyBuilder Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Specify the nullability of this property
        /// </summary>
        public PropertyBuilder Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify that this property is read-only
        /// </summary>
        public PropertyBuilder ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the property formula
        /// </summary>
        /// <param name="formula">Formula</param>
        public PropertyBuilder Formula(string formula)
        {
            mapping.Formula = formula;
            return this;
        }

        /// <summary>
        /// Specify the lazy-loading behaviour
        /// </summary>
        public PropertyBuilder LazyLoad()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify an index name
        /// </summary>
        /// <param name="index">Index name</param>
        public PropertyBuilder Index(string index)
        {
            columnAttributes.Set(x => x.Index, index);
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <typeparam name="TCustomtype">A type which implements <see cref="IUserType"/>.</typeparam>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyBuilder CustomType<TCustomtype>()
        {
            return CustomType(typeof(TCustomtype));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyBuilder CustomType(Type type)
        {
            if (typeof(ICompositeUserType).IsAssignableFrom(type))
                AddColumnsFromCompositeUserType(type);

            return CustomType(TypeMapping.GetTypeString(type));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyBuilder CustomType(string type)
        {
            mapping.Type = new TypeReference(type);
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="typeFunc">A function which returns a type which implements <see cref="IUserType"/>. The argument of the function is the mapped property type</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyBuilder CustomType(Func<Type, Type> typeFunc)
        {
            var type = typeFunc.Invoke(mapping.Member.PropertyType);

            if (typeof(ICompositeUserType).IsAssignableFrom(type))
                AddColumnsFromCompositeUserType(type);

            return CustomType(TypeMapping.GetTypeString(type));
        }

        private void AddColumnsFromCompositeUserType(Type compositeUserType)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(compositeUserType);

            foreach (var name in inst.PropertyNames)
            {
                Columns.Add(name);
            }
        }

        /// <summary>
        /// Specify a custom SQL type
        /// </summary>
        /// <param name="sqlType">SQL type</param>
        public PropertyBuilder CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        /// <summary>
        /// Specify that this property has a unique constranit
        /// </summary>
        public PropertyBuilder Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify decimal precision
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public PropertyBuilder Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        /// <summary>
        /// Specify decimal scale
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public PropertyBuilder Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        /// <summary>
        /// Specify a default value
        /// </summary>
        /// <param name="value">Default value</param>
        public PropertyBuilder Default(string value)
        {
            columnAttributes.Set(x => x.Default, value);
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public PropertyBuilder UniqueKey(string keyName)
        {
            columnAttributes.Set(x => x.UniqueKey, keyName);
            return this;
        }

        /// <summary>
        /// Specify that this property is optimistically locked
        /// </summary>
        public PropertyBuilder OptimisticLock()
        {
            mapping.OptimisticLock = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PropertyBuilder Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public PropertyBuilder Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }

        TypeReference GetDefaultType()
        {
            var propertyType = mapping.Member.PropertyType;
            var type = new TypeReference(propertyType);

            if (propertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(propertyType));

            if (propertyType.IsNullable() && propertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(propertyType.GetGenericArguments()[0]));

            return type;
        }
    }
}
