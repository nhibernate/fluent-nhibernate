using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToOneInstance : IOneToOneInspector
    {
        IAccessInstance Access { get; }
        ICascadeInstance Cascade { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IOneToOneInstance Not { get; }
        IFetchInstance Fetch { get; }
        void Class<T>();
        void Class(Type type);
        void Constrained();
        void ForeignKey(string key);
        void LazyLoad();
        void PropertyRef(string propertyName);
    }
}