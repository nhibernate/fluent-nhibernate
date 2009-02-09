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
        IProperty AutoNumber();
        IProperty WithLengthOf(int length);
        IProperty Nullable();
        IProperty ReadOnly();
        IProperty FormulaIs(string forumla);
        IProperty CustomTypeIs<T>() where T : IUserType;
        IProperty CustomTypeIs(Type type);
        IProperty CustomTypeIs(string typeName);
        IProperty CustomSqlTypeIs(string sqlType);
        IProperty Unique();

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        IProperty UniqueKey(string keyName);

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        IProperty Not { get; }
    }
}