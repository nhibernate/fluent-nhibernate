using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

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
        private string _columnName;
        private readonly AccessStrategyBuilder<PropertyMap> access;

        public PropertyMap(PropertyInfo property, bool parentIsRequired, Type parentType)
        {
            access = new AccessStrategyBuilder<PropertyMap>(this);

            _property = property;
            _parentIsRequired = parentIsRequired;
            _columnName = property.Name;
            _parentType = parentType;

            _columnProperties.Store("name", _columnName);
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
            visitor.Conventions.AlterMap(this);

            XmlElement element = classElement.AddElement("property")
                .WithAtt("name", _property.Name)
                .WithAtt("column", _columnName)
                .WithProperties(_extendedProperties);


            element.AddElement("column").WithProperties(_columnProperties);

            foreach (var action in _alterations)
            {
                action(element);
            }
        }

        public int Level
        {
            get { return 2; }
        }

        #endregion

        #region IProperty Members

        public string ColumnName()
        {
            return _columnName;
        }

        public void AddAlteration(Action<XmlElement> action)
        {
            _alterations.Add(action);
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

        #endregion

        public PropertyMap TheColumnNameIs(string name)
        {
            _columnName = name;

            _columnProperties.Remove("column");
            _columnProperties.Store("name", _columnName);

            return this;
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyMap> Access
        {
            get { return access; }
        }

        public PropertyMap ValueIsAutoNumber()
        {
            _extendedProperties.Store("insert", "true");

            return this;
        }

        public PropertyMap WithLengthOf(int length)
        {
            if (this._property.PropertyType == typeof(string))
                this.AddAlteration(x => x.SetAttribute("length", length.ToString()));
            else
                throw new InvalidOperationException(String.Format("{0} is not a string.", this._property.Name));
            return this;
        }

        public PropertyMap CanNotBeNull()
        {
            this.AddAlteration(x => x.SetAttribute("not-null", "true"));
            return this;
        }

        public PropertyMap AsReadOnly()
        {
            this.AddAlteration(x => x.SetAttribute("insert", "false"));
            this.AddAlteration(x => x.SetAttribute("update", "false"));

            return this;
        }

        public PropertyMap FormulaIs(string forumla) 
        {
            this.AddAlteration(x => x.SetAttribute("formula", forumla));

            return this;
        }
    }
}
