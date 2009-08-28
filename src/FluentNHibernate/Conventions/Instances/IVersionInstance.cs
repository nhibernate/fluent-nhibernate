using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IVersionInstance : IVersionInspector
    {
        new IAccessInstance Access { get; }
        new IGeneratedInstance Generated { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IVersionInstance Not { get; }
        void Column(string columnName);
        void UnsavedValue(string unsavedValue);
        void Length(int length);
        void Precision(int precision);
        void Scale(int scale);
        void Nullable();
        void Unique();
        void UniqueKey(string keyColumns);
        void CustomSqlType(string sqlType);
        void Index(string index);
        void Check(string constraint);
        void Default(object value);
        void CustomType<T>();
        void CustomType(Type type);
        void CustomType(string type);
    }
}