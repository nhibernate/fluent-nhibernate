using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IPropertyInstance 
        : IPropertyInspector, IInsertInstance, 
          IUpdateInstance, IReadOnlyInstance, 
          INullableInstance, IAccessInstance
    {
        new IAccessStrategyBuilder Access { get; }
        IPropertyInstance Not { get; }
        void CustomTypeIs<T>();
        void CustomTypeIs(TypeReference type);
        void CustomTypeIs(Type type);
        void CustomTypeIs(string type);
        void CustomSqlTypeIs(string sqlType);
        void Unique();
        void UniqueKey(string keyName);
        void ColumnName(string columnName);
    }
}