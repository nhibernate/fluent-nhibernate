using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyMap : IProperty, IAccessStrategy<PropertyMap>
    {
        private readonly Type parentType;
        private readonly AccessStrategyBuilder<PropertyMap> access;
        private readonly ColumnNameCollection<IProperty> columnNames;
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private bool nextBool = true;

        private readonly PropertyMapping mapping;

        public PropertyMap(PropertyMapping mapping, Type parentType)
        {
            columnNames = new ColumnNameCollection<IProperty>(this);
            access = new AccessStrategyBuilder<PropertyMap>(this, value => mapping.Access = value);

            this.mapping = mapping;
            this.parentType = parentType;
        }

        public PropertyMapping GetPropertyMapping()
        {
            if (columnNames.List().Count == 0)
                columnNames.Add(mapping.Name);

            foreach (var column in columnNames.List())
            {
                var columnMapping = new ColumnMapping(columnAttributes.Clone())
                {
                    Name = column
                };

                mapping.AddColumn(columnMapping);
            }

            if (!mapping.Attributes.IsSpecified(x => x.Type))
                mapping.Type = TypeMapping.GetTypeString(Property.PropertyType);

            return mapping;
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public Type PropertyType
        {
            get { return mapping.PropertyInfo.PropertyType; }
        }

        public Type EntityType
        {
            get { return parentType; }
        }

        public IProperty ColumnName(string columnName)
        {
            ColumnNames.Clear();
            ColumnNames.Add(columnName);
            return this;
        }

        public ColumnNameCollection<IProperty> ColumnNames
        {
            get { return columnNames; }
        }

        IColumnNameCollection IProperty.ColumnNames
        {
            get { return ColumnNames; }
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyMap> Access
        {
            get { return access; }
        }

        IAccessStrategyBuilder IProperty.Access
        {
            get { return Access; }
        }

        public IProperty Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;

            return this;
        }

        public IProperty Update()
        {
            mapping.Update = nextBool;
            nextBool = true;

            return this;
        }

        public IProperty WithLengthOf(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public IProperty Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public IProperty ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public IProperty FormulaIs(string formula) 
        {
            mapping.Formula = formula;
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <typeparam name="TCustomtype">A type which implements <see cref="IUserType"/>.</typeparam>
        /// <returns>This property mapping to continue the method chain</returns>
        public IProperty CustomTypeIs<TCustomtype>()
        {
            return CustomTypeIs(typeof(TCustomtype));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public IProperty CustomTypeIs(Type type)
        {
            if (typeof(ICompositeUserType).IsAssignableFrom(type))
                AddColumnsFromCompositeUserType(type);

            return CustomTypeIs(TypeMapping.GetTypeString(type));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public IProperty CustomTypeIs(string type)
        {
            mapping.Type = type;

            return this;
        }

        private void AddColumnsFromCompositeUserType(Type compositeUserType)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(compositeUserType);

            foreach (var name in inst.PropertyNames)
            {
                ColumnNames.Add(name);
            }
        }

        public IProperty CustomSqlTypeIs(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        public IProperty Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public IProperty UniqueKey(string keyName)
        {
            mapping.UniqueKey = keyName;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public IProperty Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this property mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        void IHasAttributes.SetAttribute(string name, string value)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttributes(Attributes atts)
        {
            throw new NotSupportedException("Obsolete");
        }
    }
}
