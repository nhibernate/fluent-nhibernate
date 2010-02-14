using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void Column(string column);
        new void UnsavedValue(string unsavedValue);
        new void Length(int length);
        void CustomType(string type); 
        void CustomType(Type type); 
        void CustomType<T>(); 
        new IAccessInstance Access { get; }
        IGeneratorInstance GeneratedBy { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IIdentityInstance Not { get; }
        new void Precision(int precision);
        new void Scale(int scale);
        new void Nullable();
        new void Unique();
        new void UniqueKey(string columns);
        void CustomSqlType(string sqlType);
        new void Index(string index);
        new void Check(string constraint);
        new void Default(object value);
    }
}