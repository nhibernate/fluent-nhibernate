using System;
using System.Reflection;
using System.Xml;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public interface IProperty : IHasAttributes
    {
        void AddAlteration(Action<XmlElement> action);
        void SetAttributeOnColumnElement(string name, string value);
        Type PropertyType { get; }
        string ColumnName();
        Type ParentType { get; }
        PropertyInfo Property { get; }
        bool ParentIsRequired { get; }

        // Possibly should be moved to IHasAttributes.
        bool HasAttribute(string name);
    }
}