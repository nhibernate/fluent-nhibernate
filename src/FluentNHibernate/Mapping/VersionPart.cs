using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IVersion : IMappingPart
    {
        AccessStrategyBuilder<VersionPart> Access { get; }
        string GetColumnName();
        Type EntityType { get; }
        PropertyInfo Property { get; }
        IVersion ColumnName(string name);
        IVersion NeverGenerated();
    }

    public class VersionPart : IVersion
    {
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private readonly AccessStrategyBuilder<VersionPart> _access;
        private readonly Dictionary<string,string> _properties;
        private bool _neverGenerated;

        public VersionPart(Type entity, PropertyInfo property)
        {
            EntityType = entity;
            _access = new AccessStrategyBuilder<VersionPart>(this);
            _properties = new Dictionary<string, string>();
            Property = property;
        }

        public void SetAttribute(string name, string value)
        {
            _properties.Add(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var versionElement = classElement 
                                    .AddElement("version")
                                    .WithProperties(_properties);
            
            versionElement 
                .WithAtt("name", Property.Name);

            if (_neverGenerated)
                versionElement.WithAtt("generated", "never");

            if (Property.PropertyType == typeof(DateTime))
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

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        public IVersion ColumnName(string name)
        {
            SetAttribute("column", name);
            return this;
        }

        public string GetColumnName()
        {
            if (_properties.ContainsKey("column"))
                return _properties["column"];

            return null;
        }

        public IVersion NeverGenerated()
        {
            _neverGenerated = true;
            return this;
        }
    }
}