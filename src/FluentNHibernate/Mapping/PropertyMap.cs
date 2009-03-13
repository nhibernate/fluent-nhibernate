using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyMap : IMappingPart, IProperty, IAccessStrategy<PropertyMap>
    {
        private readonly List<Action<XmlElement>> _alterations = new List<Action<XmlElement>>();
        private readonly Cache<string, string> _extendedProperties = new Cache<string, string>();
        private readonly Cache<string, string> _columnProperties = new Cache<string, string>();
        private readonly Type _parentType;
        private readonly PropertyInfo _property;
        private readonly bool _parentIsRequired;
        private readonly AccessStrategyBuilder<PropertyMap> access;
        private bool nextBool = true;
        private readonly List<string> columnNames = new List<string>();

        public PropertyMap(PropertyInfo property, bool parentIsRequired, Type parentType)
        {
            access = new AccessStrategyBuilder<PropertyMap>(this);

            _property = property;
            _parentIsRequired = parentIsRequired;
            _parentType = parentType;
        }

        public bool ParentIsRequired
        {
            get { return _parentIsRequired; }
        }

        public PropertyInfo Property
        {
            get { return _property; }
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("property")
                .WithAtt("name", _property.Name)
                .WithProperties(_extendedProperties);

            AddColumnElements(element);

            foreach (var action in _alterations)
            {
                action(element);
            }
        }

        private void AddColumnElements(XmlNode element)
        {
            if (columnNames.Count == 0)
                columnNames.Add(_property.Name);

            columnNames.ForEach(column =>
                element.AddElement("column")
                    .WithAtt("name", column)
                    .WithProperties(_columnProperties));
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

        public void AddAlteration(Action<XmlElement> action)
        {
            _alterations.Add(action);
        }

        public bool HasAttribute(string name)
        {
            return _extendedProperties.Has(name);
        }

        public string GetAttribute(string name)
        {
            return _extendedProperties.Get(name);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this property mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            _extendedProperties.Store(name, value);
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

        public Type PropertyType
        {
            get { return _property.PropertyType; }
        }

        public Type ParentType
        {
            get { return _parentType; }
        }

        public IProperty ColumnName(string name)
        {
            columnNames.Add(name);
            return this;
        }

        public IProperty ColumnNames(params string[] names)
        {
            foreach (var name in names)
            {
                ColumnName(name);
            }

            return this;
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyMap> Access
        {
            get { return access; }
        }

        public IProperty AutoNumber()
        {
            _extendedProperties.Store("insert", nextBool.ToString().ToLowerInvariant());
            nextBool = true;

            return this;
        }

        public IProperty WithLengthOf(int length)
        {
            if (CanApplyLengthAttribute())
                this.AddAlteration(x => x.SetColumnProperty("length", length.ToString()));
            else
                throw new InvalidOperationException(String.Format("{0} is not a string.", this._property.Name));
            return this;
        }

    	private bool CanApplyLengthAttribute()
    	{
    		var propertyType = this._property.PropertyType;
    		return  propertyType == typeof(string) || propertyType == typeof(decimal);
    	}

        public IProperty Nullable()
        {
            SetAttributeOnColumnElement("not-null", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        public IProperty ReadOnly()
        {
            _extendedProperties.Store("insert", (!nextBool).ToString().ToLowerInvariant());
            _extendedProperties.Store("update", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        public IProperty FormulaIs(string forumla) 
        {
            this.AddAlteration(x => x.SetAttribute("formula", forumla));

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

            return CustomTypeIs(type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type name.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public IProperty CustomTypeIs(string type)
        {
            SetAttribute("type", type);
            return this;
        }

        private void AddColumnsFromCompositeUserType(Type compositeUserType)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(compositeUserType);

            foreach (var name in inst.PropertyNames)
            {
                ColumnName(name);
            }
        }

        public IProperty CustomSqlTypeIs(string sqlType)
        {
            this.AddAlteration(x => x.SetColumnProperty("sql-type", sqlType));
            return this;
        }

        public IProperty Unique()
        {
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
