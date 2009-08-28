using System;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyPart : IPropertyMappingProvider
    {
        private readonly PropertyInfo property;
        private readonly Type parentType;
        private readonly AccessStrategyBuilder<PropertyPart> access;
        private readonly PropertyGeneratedBuilder generated;
        private readonly ColumnNameCollection<PropertyPart> columns;
        private readonly AttributeStore<PropertyMapping> attributes = new AttributeStore<PropertyMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private bool nextBool = true;

        public PropertyPart(PropertyInfo property, Type parentType)
        {
            columns = new ColumnNameCollection<PropertyPart>(this);
            access = new AccessStrategyBuilder<PropertyPart>(this, value => attributes.Set(x => x.Access, value));
            generated = new PropertyGeneratedBuilder(this, value => attributes.Set(x => x.Generated, value));

            this.property = property;
            this.parentType = parentType;
        }

        public PropertyGeneratedBuilder Generated
        {
            get { return generated; }
        }

        PropertyMapping IPropertyMappingProvider.GetPropertyMapping()
        {
            var mapping = new PropertyMapping(attributes.CloneInner())
            {
                ContainingEntityType = parentType,
                PropertyInfo = property
            };

            if (columns.List().Count == 0)
                mapping.AddDefaultColumn(CreateColumn(mapping.PropertyInfo.Name));

            foreach (var column in columns.List())
            {
                var columnMapping = CreateColumn(column);

                mapping.AddColumn(columnMapping);
            }

            if (!mapping.IsSpecified("Name"))
                mapping.Name = mapping.PropertyInfo.Name;

            if (!mapping.IsSpecified("Type"))
                mapping.SetDefaultValue("Type", GetDefaultType());

            return mapping;
        }

        private TypeReference GetDefaultType()
        {
            var type = new TypeReference(property.PropertyType);

            if (property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType));

            if (property.PropertyType.IsNullable() && property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));

            return type;
        }

        private ColumnMapping CreateColumn(string column)
        {
            var columnMapping = new ColumnMapping(columnAttributes.CloneInner())
            {
                Name = column
            };

            if (!columnMapping.IsSpecified("NotNull") && property.PropertyType.IsNullable() && property.PropertyType.IsEnum())
                columnMapping.SetDefaultValue(x => x.NotNull, false);

            return columnMapping;
        }

        public PropertyPart Column(string columnName)
        {
            Columns.Clear();
            Columns.Add(columnName);
            return this;
        }

        public ColumnNameCollection<PropertyPart> Columns
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

        public PropertyPart Insert()
        {
            attributes.Set(x => x.Insert, nextBool);
            nextBool = true;

            return this;
        }

        public PropertyPart Update()
        {
            attributes.Set(x => x.Update, nextBool);
            nextBool = true;

            return this;
        }

        public PropertyPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public PropertyPart Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public PropertyPart ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            attributes.Set(x => x.Update, !nextBool);
            nextBool = true;
            return this;
        }

        public PropertyPart Formula(string formula) 
        {
            attributes.Set(x => x.Formula, formula);
            return this;
        }

        public PropertyPart LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public PropertyPart Index(string index)
        {
            columnAttributes.Set(x => x.Index, index);
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
            attributes.Set(x => x.Type, new TypeReference(type));

            return this;
        }

        private void AddColumnsFromCompositeUserType(Type compositeUserType)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(compositeUserType);

            foreach (var name in inst.PropertyNames)
            {
                Columns.Add(name);
            }
        }

        public PropertyPart CustomSqlType(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        public PropertyPart Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        public PropertyPart Precision(int precision)
        {
            columnAttributes.Set(x => x.Precision, precision);
            return this;
        }

        public PropertyPart Scale(int scale)
        {
            columnAttributes.Set(x => x.Scale, scale);
            return this;
        }

        public PropertyPart Default(string value)
        {
            columnAttributes.Set(x => x.Default, value);
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public PropertyPart UniqueKey(string keyName)
        {
            columnAttributes.Set(x => x.UniqueKey, keyName);
            return this;
        }

        public PropertyPart OptimisticLock()
        {
            attributes.Set(x => x.OptimisticLock, nextBool);
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

        public PropertyPart Check(string constraint)
        {
            columnAttributes.Set(x => x.Check, constraint);
            return this;
        }
    }
}
