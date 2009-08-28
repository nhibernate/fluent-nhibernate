using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void Column(string column);
        void UnsavedValue(string unsavedValue);
        void Length(int length);
        void CustomType(string type); 
        void CustomType(Type type); 
        void CustomType<T>(); 
        new IAccessInstance Access { get; }
        IGeneratorInstance GeneratedBy { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IIdentityInstance Not { get; }
        void Precision(int precision);
        void Scale(int scale);
        void Nullable();
        void Unique();
        void UniqueKey(string columns);
        void CustomSqlType(string sqlType);
        void Index(string index);
        void Check(string constraint);
        void Default(object value);
    }
}