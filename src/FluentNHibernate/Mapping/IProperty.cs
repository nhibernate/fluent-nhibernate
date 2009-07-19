using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IProperty
    {
        Type EntityType { get; }
        Type PropertyType { get; }
        IAccessStrategyBuilder Access { get; }
        IProperty ColumnName(string columnName);
        IColumnNameCollection ColumnNames { get; }

        IProperty Insert();
        IProperty Update();
        IProperty Length(int length);
        IProperty Nullable();
        IProperty ReadOnly();
        IProperty Formula(string forumla);
        IProperty CustomType<T>();
        IProperty CustomType(Type type);
        IProperty CustomType(string type);
        IProperty CustomSqlType(string sqlType);
        IProperty Unique();
        IProperty OptimisticLock();

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        IProperty UniqueKey(string keyName);

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        IProperty Not { get; }
        PropertyGeneratedBuilder Generated { get; }
        PropertyMapping GetPropertyMapping();
    }
}