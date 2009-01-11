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
        IProperty TheColumnNameIs(string name);
        IProperty ValueIsAutoNumber();
        IProperty WithLengthOf(int length);
        IProperty CanNotBeNull();
        IProperty AsReadOnly();
        IProperty FormulaIs(string forumla);
        IProperty CustomTypeIs(Type type);
        IProperty CustomTypeIs(string typeName);
        IProperty CustomSqlTypeIs(string sqlType);
        IProperty WithUniqueConstraint();
    }
}