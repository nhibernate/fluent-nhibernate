using System;
using System.Diagnostics;
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IPropertyInstance Not { get; }
        void CustomType<T>();
        void CustomType<T>(string columnPrefix);
        void CustomType(TypeReference type);
        void CustomType(TypeReference type, string columnPrefix);
        void CustomType(Type type);
        void CustomType(Type type, string columnPrefix);
        void CustomType(string type);
        void CustomType(string type, string columnPrefix);
        void CustomSqlType(string sqlType);
        new void Precision(int precision);
        new void Scale(int scale);
        new void Default(string value);
        new void Unique();
        new void UniqueKey(string keyName);
        void Column(string columnName);
        new void Formula(string formula);
        new IGeneratedInstance Generated { get; }
        new void OptimisticLock();
        new void Length(int length);
        new void LazyLoad();
        new void Index(string value);
        new void Check(string constraint);
    }
}