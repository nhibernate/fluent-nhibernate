using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void Column(string column);
        void UnsavedValue(string unsavedValue);
        void Length(int length);
        void Type(Type type); 
        void Type<T>(); 
        new IAccessInstance Access { get; }
        IGeneratorInstance GeneratedBy { get; }
    }
}