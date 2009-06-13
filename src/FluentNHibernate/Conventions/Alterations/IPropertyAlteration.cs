using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
{
    public interface IPropertyAlteration
        : IInsertAlteration, IUpdateAlteration,
          IReadOnlyAlteration, INullableAlteration,
          IAccessAlteration
    {
        void CustomTypeIs<T>();
        void CustomTypeIs(TypeReference type);
        void CustomTypeIs(Type type);
        void CustomTypeIs(string type);
        void CustomSqlTypeIs(string sqlType);
        void Unique();
        void UniqueKey(string keyName);

        IPropertyAlteration Not { get; }
        void ColumnName(string columnName);
    }
}
