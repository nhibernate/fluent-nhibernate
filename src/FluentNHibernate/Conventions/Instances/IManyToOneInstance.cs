using System;
using System.Diagnostics;
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IManyToOneInstance Not { get; }
        new INotFoundInstance NotFound { get; }
        void Index(string index);
        new void Insert();
        new void LazyLoad();
        new void Nullable();
        new void PropertyRef(string property);
        void ReadOnly();
        void Unique();
        void UniqueKey(string key);
        new void Update();
        new void ForeignKey(string key);
        new void Formula(string formula);

        void OverrideInferredClass(Type type);
    }
}