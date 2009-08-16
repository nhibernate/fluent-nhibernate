using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToOneInstance : IManyToOneInspector
    {
        void Column(string columnName);
        void CustomClass<T>();
        void CustomClass(Type type);
        new IAccessInstance Access { get; }
        new ICascadeInstance Cascade { get; }
        new IFetchInstance Fetch { get; }
        IManyToOneInstance Not { get; }
        new INotFoundInstance NotFound { get; }
        void Index(string index);
        void Insert();
        void LazyLoad();
        void Nullable();
        void PropertyRef(string property);
        void ReadOnly();
        void Unique();
        void UniqueKey(string key);
        void Update();
        void ForeignKey(string key);
    }
}