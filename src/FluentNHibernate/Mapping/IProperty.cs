using System;
using System.Reflection;

namespace FluentNHibernate.Mapping
{
    public interface IProperty : IMappingPart
    {
        Type EntityType { get; }
        Type PropertyType { get; }
        PropertyInfo Property { get; }
        IAccessStrategyBuilder Access { get; }
        IProperty ColumnName(string columnName);
        IColumnNameCollection ColumnNames { get; }

        IProperty Insert();
        IProperty Update();
        IProperty WithLengthOf(int length);
        IProperty Nullable();
        IProperty ReadOnly();
        IProperty FormulaIs(string forumla);
        IProperty CustomTypeIs<T>();
        IProperty CustomTypeIs(Type type);
        IProperty CustomTypeIs(string type);
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