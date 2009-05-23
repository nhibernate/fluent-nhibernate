using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

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
        IVersion UnsavedValue(string value);
    }

    public class VersionPart : IVersion
    {
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly Cache<string, string> properties;
        private bool neverGenerated;

        public VersionPart(Type entity, PropertyInfo property)
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<VersionPart>(this, value => SetAttribute("access", value));
            properties = new Cache<string, string>();
            Property = property;
            SetAttribute("name", Property.Name);
        }

        public void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
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
                                    .WithProperties(properties);

            if (neverGenerated)
            { versionElement.WithAtt("generated", "never"); }

            if (Property.PropertyType == typeof(DateTime))
            { versionElement.WithAtt("type", "timestamp"); }
        }

        public AccessStrategyBuilder<VersionPart> Access
        {
            get
            {
                return access;
            }
        }

        public int LevelWithinPosition
        {
            get { return 4; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.First; }
        }

        public IVersion ColumnName(string name)
        {
            SetAttribute("column", name);
            return this;
        }

        public string GetColumnName()
        {
            if (properties.Has("column"))
            { return properties.Get("column"); }

            return null;
        }

        public IVersion NeverGenerated()
        {
            neverGenerated = true;
            return this;
        }

        public IVersion UnsavedValue(string value)
        {
            SetAttribute("unsaved-value", value);
            return this;
        }
    }
}