using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IPropertyInstance 
        : IPropertyInspector, IInsertInstance, 
          IUpdateInstance, IReadOnlyInstance, 
          INullableInstance
    {
        new IAccessInstance Access { get; }
        IPropertyInstance Not { get; }
        void CustomType<T>();
        void CustomType(TypeReference type);
        void CustomType(Type type);
        void CustomType(string type);
        void CustomSqlType(string sqlType);
        void Precision(int precision);
        void Scale(int scale);
        void Default(string value);
        void Unique();
        void UniqueKey(string keyName);
        void Column(string columnName);
        void Formula(string formula);
        new IGeneratedInstance Generated { get; }
        void OptimisticLock();
        void Length(int length);
        void LazyLoad();
        void Index(string value);
    }
}