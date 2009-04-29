using System;
using System.Reflection;
using System.Xml;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public interface IProperty : IMappingPart
    {
        void SetAttributeOnColumnElement(string name, string value);
        Type EntityType { get; }
        Type PropertyType { get; }
        PropertyInfo Property { get; }
        bool ParentIsRequired { get; }
        IAccessStrategyBuilder Access { get; }
        bool HasAttribute(string name);

        IColumnNameCollection ColumnNames { get; }

        IProperty AutoNumber();
        IProperty WithLengthOf(int length);
        IProperty Nullable();
        IProperty ReadOnly();
        IProperty FormulaIs(string forumla);
        IProperty CustomTypeIs<T>();
        IProperty CustomTypeIs(Type type);
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