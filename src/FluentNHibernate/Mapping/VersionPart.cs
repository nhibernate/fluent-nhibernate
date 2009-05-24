using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IVersion : IMappingPart
    {
        AccessStrategyBuilder<VersionPart> Access { get; }
        string GetColumnName();
        Type EntityType { get; }
        PropertyInfo Property { get; }
        VersionGeneratedBuilder<IVersion> Generated { get; }
        IVersion ColumnName(string name);
        IVersion UnsavedValue(string value);
        VersionMapping GetVersionMapping();
    }

    public class VersionPart : IVersion
    {
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly Cache<string, string> properties;
        private readonly VersionGeneratedBuilder<IVersion> generated;

        private readonly VersionMapping mapping = new VersionMapping();

        public VersionPart(Type entity, PropertyInfo property)
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<VersionPart>(this, value => mapping.Access = value);
            generated = new VersionGeneratedBuilder<IVersion>(this, value => mapping.Generated = value);
            properties = new Cache<string, string>();
            Property = property;
        }

        VersionMapping IVersion.GetVersionMapping()
        {
            mapping.Name = Property.Name;
            mapping.Type = Property.PropertyType == typeof(DateTime) ? "timestamp" : Property.PropertyType.AssemblyQualifiedName;

            if (!mapping.Attributes.IsSpecified(x => x.Column))
                mapping.Column = Property.Name;

            return mapping;
        }

        public VersionGeneratedBuilder<IVersion> Generated
        {
            get { return generated; }
        }

        public AccessStrategyBuilder<VersionPart> Access
        {
            get { return access; }
        }

        public IVersion ColumnName(string name)
        {
            mapping.Column = name;
            return this;
        }

        public string GetColumnName()
        {
            return mapping.Column;
        }

        public IVersion UnsavedValue(string value)
        {
            mapping.UnsavedValue = value;
            return this;
        }

        void IHasAttributes.SetAttribute(string name, string value)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttributes(Attributes atts)
        {
            throw new NotSupportedException("Obsolete");
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
    }
}