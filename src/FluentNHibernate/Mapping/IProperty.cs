using System;
using System.Reflection;
using System.Xml;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public interface IProperty
    {
        void AddAlteration(Action<XmlElement> action);
        void SetAttributeOnPropertyElement(string name, string key);
        void SetAttributeOnColumnElement(string name, string value);
        Type PropertyType { get; }
        string ColumnName();
        Type ParentType { get; }
        PropertyInfo Property { get; }
        bool ParentIsRequired { get; }
    }
}