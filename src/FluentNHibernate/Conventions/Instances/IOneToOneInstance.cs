using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToOneInstance : IOneToOneInspector
    {
        new IAccessInstance Access { get; }
        new ICascadeInstance Cascade { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IOneToOneInstance Not { get; }
        new IFetchInstance Fetch { get; }
        new void Class<T>();
        new void Class(Type type);
        new void Constrained();
        new void ForeignKey(string key);
        new void LazyLoad();
        new void PropertyRef(string propertyName);

        void OverrideInferredClass(Type type);
    }
}