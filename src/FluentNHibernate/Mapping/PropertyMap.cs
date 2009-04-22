using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyMap : IProperty, IAccessStrategy<PropertyMap>
    {
        private readonly List<Action<XmlElement>> _alterations = new List<Action<XmlElement>>();
        private readonly Cache<string, string> _extendedProperties = new Cache<string, string>();
        private readonly Cache<string, string> _columnProperties = new Cache<string, string>();
        private readonly Type _parentType;
        private readonly bool _parentIsRequired;
        private readonly AccessStrategyBuilder<PropertyMap> access;
        private bool nextBool = true;
        private readonly ColumnNameCollection<IProperty> columnNames;
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();

        private readonly PropertyMapping mapping;
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public PropertyMap(PropertyMapping mapping)
        {
            columnNames = new ColumnNameCollection<IProperty>(this);
            access = new AccessStrategyBuilder<PropertyMap>(this);

            this.mapping = mapping;
        }

        public bool ParentIsRequired
        {
            get { return _parentIsRequired; }
        }

        #region IMappingPart Members

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

            foreach (var attribute in unmigratedAttributes)
                mapping.AddUnmigratedAttribute(attribute.Key, attribute.Value);

            return mapping;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("property")
                .WithAtt("name", mapping.Name)
                .WithProperties(_extendedProperties);

            AddColumnElements(element);

            foreach (var action in _alterations)
            {
                action(element);
            }
        }

        private void AddColumnElements(XmlNode element)
        {
            if (columnNames.List().Count == 0)
                columnNames.Add(Property.Name);

            foreach (var column in columnNames.List())
            {
                element.AddElement("column")
                    .WithAtt("name", column)
                    .WithProperties(_columnProperties);
            }
        }

        public int Level
        {
            get { return 2; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        #endregion

        public bool HasAttribute(string name)
        {
            return _extendedProperties.Has(name) || unmigratedAttributes.ContainsKey(name);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this property mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            _extendedProperties.Store(name, value);
            unmigratedAttributes.Add(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void SetAttributeOnColumnElement(string name, string value)
        {
            _columnProperties.Store(name, value);
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
            get { return _parentType; }
        }

        public void ColumnName(string columnName)
        {
            ColumnNames.Clear();
            ColumnNames.Add(columnName);
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

        public IProperty AutoNumber()
        {
            mapping.Insert = nextBool;
            nextBool = true;

            return this;
        }

        public IProperty WithLengthOf(int length)
        {
            _columnProperties.Store("length", length.ToString());
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public IProperty Nullable()
        {
            _columnProperties.Store("not-null", (!nextBool).ToString().ToLowerInvariant());
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public IProperty ReadOnly()
        {
            _extendedProperties.Store("insert", (!nextBool).ToString().ToLowerInvariant());
            _extendedProperties.Store("update", (!nextBool).ToString().ToLowerInvariant());
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public IProperty FormulaIs(string formula) 
        {
            mapping.Formula = formula;
            _extendedProperties.Store("formula", formula);
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <typeparam name="CUSTOMTYPE">A type which implements <see cref="IUserType"/>.</typeparam>
        /// <returns>This property mapping to continue the method chain</returns>
        public IProperty CustomTypeIs<CUSTOMTYPE>()
        {
            return CustomTypeIs(typeof(CUSTOMTYPE));
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

            mapping.Type = type;
            _extendedProperties.Store("type", TypeMapping.GetTypeString(type));

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
            _columnProperties.Store("sql-type", sqlType);
            return this;
        }

        public IProperty Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            _columnProperties.Store("unique", nextBool.ToString().ToLowerInvariant());
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
            _extendedProperties.Store("unique-key", keyName);
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
    }
}
