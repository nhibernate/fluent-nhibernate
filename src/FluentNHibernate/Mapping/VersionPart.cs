using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IMappingPart
    {
        private readonly PropertyInfo _property;
        private string _columnName;
        private readonly AccessStrategyBuilder<VersionPart> _access;
        private Dictionary<string,string> _properties;
        private bool _neverGenerated;

        public VersionPart(PropertyInfo property)
        {
            _access = new AccessStrategyBuilder<VersionPart>(this);
            _properties = new Dictionary<string, string>();
            _property = property;
            _columnName = _property.Name;
        }

        public void SetAttribute(string name, string value)
        {
            _properties.Add(name, value);
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var versionElement = classElement 
                                    .AddElement("version")
                                    .WithProperties(_properties);
            
            versionElement 
                .WithAtt("column", _columnName)
                .WithAtt("name", _property.Name);

            if (_neverGenerated)
                versionElement.WithAtt("generated", "never");

            if (_property.PropertyType == typeof(DateTime))
                versionElement.WithAtt("type", "timestamp");
        }

        public AccessStrategyBuilder<VersionPart> Access 
        { 
            get
            {
                return _access;
            }
        }

        public int Level
        {
            get { return 1; }
        }

        public VersionPart TheColumnNameIs(string columnName)
        {
            _columnName = columnName;
            return this;
        }

        public VersionPart NeverGenerated()
        {
            _neverGenerated = true;
            return this;
        }
    }
}