using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ShadeTree.Core;
using ShadeTree.Validation;

namespace ShadeTree.DomainModel.Mapping
{
    public class PropertyMap : IMappingPart, IProperty
    {
        private readonly List<Action<XmlElement>> _alterations = new List<Action<XmlElement>>();
        private readonly Cache<string, string> _extendedProperties = new Cache<string, string>();
        private readonly Cache<string, string> _columnProperties = new Cache<string, string>();
        private readonly Type _parentType;
        private readonly PropertyInfo _property;
        private readonly bool _parentIsRequired;
        private string _columnName;

        public PropertyMap(PropertyInfo property, bool parentIsRequired, Type parentType)
        {
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

        public void SetAttributeOnPropertyElement(string name, string key)
        {
            _extendedProperties.Store(name, key);
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

        public PropertyMap ValueIsAutoNumber()
        {
            _extendedProperties.Store("insert", "true");

            return this;
        }
    }
}