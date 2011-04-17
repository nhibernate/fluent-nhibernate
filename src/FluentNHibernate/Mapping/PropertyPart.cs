using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyPart : IPropertyMappingProvider
    {
        readonly Member member;
        readonly Type parentType;
        readonly AccessStrategyBuilder<PropertyPart> access;
        readonly PropertyGeneratedBuilder generated;
        readonly ColumnMappingCollection<PropertyPart> columns;
        readonly AttributeStore attributes = new AttributeStore();
        readonly AttributeStore columnAttributes = new AttributeStore();

        private bool nextBool = true;

        public PropertyPart(Member member, Type parentType)
        {
            columns = new ColumnMappingCollection<PropertyPart>(this);
            access = new AccessStrategyBuilder<PropertyPart>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            generated = new PropertyGeneratedBuilder(this, value => attributes.Set("Generated", Layer.UserSupplied, value));

            this.member = member;
            this.parentType = parentType;

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
        /// Specify if this property is database generated
        /// </summary>
        /// <example>
        /// Generated.Insert();
        /// </example>
        public PropertyGeneratedBuilder Generated
        {
            get { return generated; }
        }

        /// <summary>
        /// Specify the property column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public PropertyPart Column(string columnName)
        {
            Columns.Clear();
            Columns.Add(columnName);
            return this;
        }

        /// <summary>
        /// Modify the columns collection
        /// </summary>
        public ColumnMappingCollection<PropertyPart> Columns
        {
            get { return columns; }
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyPart> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Specify that this property is insertable
        /// </summary>
        public PropertyPart Insert()
        {
            attributes.Set("Insert", Layer.UserSupplied, nextBool);
            nextBool = true;

            return this;
        }

        /// <summary>
        /// Specify that this property is updatable
        /// </summary>
        public PropertyPart Update()
        {
            attributes.Set("Update", Layer.UserSupplied, nextBool);
            nextBool = true;

            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public PropertyPart Length(int length)
        {
            columnAttributes.Set("Length", Layer.UserSupplied, length);
            return this;
        }

        /// <summary>
        /// Specify the nullability of this property
        /// </summary>
        public PropertyPart Nullable()
        {
            columnAttributes.Set("NotNull", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify that this property is read-only
        /// </summary>
        public PropertyPart ReadOnly()
        {
            attributes.Set("Insert", Layer.UserSupplied, !nextBool);
            attributes.Set("Update", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the property formula
        /// </summary>
        /// <param name="formula">Formula</param>
        public PropertyPart Formula(string formula)
        {
            attributes.Set("Formula", Layer.UserSupplied, formula);
            return this;
        }

        /// <summary>
        /// Specify the lazy-loading behaviour
        /// </summary>
        public PropertyPart LazyLoad()
        {
            attributes.Set("Lazy", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify an index name
        /// </summary>
        /// <param name="index">Index name</param>
        public PropertyPart Index(string index)
        {
            columnAttributes.Set("Index", Layer.UserSupplied, index);
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <typeparam name="TCustomtype">A type which implements <see cref="IUserType"/>.</typeparam>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyPart CustomType<TCustomtype>()
        {
            return CustomType(typeof(TCustomtype));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyPart CustomType(Type type)
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
        public PropertyPart CustomType(string type)
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(type));

            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="typeFunc">A function which returns a type which implements <see cref="IUserType"/>. The argument of the function is the mapped property type</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyPart CustomType(Func<Type, Type> typeFunc)
        {
            var type = typeFunc.Invoke(member.PropertyType);

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
        public PropertyPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set("SqlType", Layer.UserSupplied, sqlType);
            return this;
        }

        /// <summary>
        /// Specify that this property has a unique constranit
        /// </summary>
        public PropertyPart Unique()
        {
            columnAttributes.Set("Unique", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify decimal precision
        /// </summary>
        /// <param name="precision">Decimal precision</param>
        public PropertyPart Precision(int precision)
        {
            columnAttributes.Set("Precision", Layer.UserSupplied, precision);
            return this;
        }

        /// <summary>
        /// Specify decimal scale
        /// </summary>
        /// <param name="scale">Decimal scale</param>
        public PropertyPart Scale(int scale)
        {
            columnAttributes.Set("Scale", Layer.UserSupplied, scale);
            return this;
        }

        /// <summary>
        /// Specify a default value
        /// </summary>
        /// <param name="value">Default value</param>
        public PropertyPart Default(string value)
        {
            columnAttributes.Set("Default", Layer.UserSupplied, value);
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public PropertyPart UniqueKey(string keyName)
        {
            columnAttributes.Set("UniqueKey", Layer.UserSupplied, keyName);
            return this;
        }

        /// <summary>
        /// Specify that this property is optimistically locked
        /// </summary>
        public PropertyPart OptimisticLock()
        {
            attributes.Set("OptimisticLock", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PropertyPart Not
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
        public PropertyPart Check(string constraint)
        {
            columnAttributes.Set("Check", Layer.UserSupplied, constraint);
            return this;
        }

        PropertyMapping IPropertyMappingProvider.GetPropertyMapping()
        {
            var mapping = new PropertyMapping(attributes.Clone())
            {
                ContainingEntityType = parentType,
                Member = member
            };

            if (columns.Count() == 0 && !mapping.IsSpecified("Formula"))
            {
                var columnMapping = new ColumnMapping(columnAttributes.Clone());
                columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
                mapping.AddColumn(Layer.Defaults, columnMapping);
            }

            foreach (var column in columns)
                mapping.AddColumn(Layer.UserSupplied, column);

            foreach (var column in mapping.Columns)
            {
                if (member.PropertyType.IsNullable() && member.PropertyType.IsEnum())
                    column.Set(x => x.NotNull, Layer.Defaults, false);

                column.MergeAttributes(columnAttributes);
            }

            mapping.Set(x => x.Name, Layer.Defaults, mapping.Member.Name);
            mapping.Set(x => x.Type, Layer.Defaults, GetDefaultType());

            return mapping;
        }

        TypeReference GetDefaultType()
        {
            var type = new TypeReference(member.PropertyType);

            if (member.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(member.PropertyType));

            if (member.PropertyType.IsNullable() && member.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(member.PropertyType.GetGenericArguments()[0]));

            return type;
        }
    }
}
