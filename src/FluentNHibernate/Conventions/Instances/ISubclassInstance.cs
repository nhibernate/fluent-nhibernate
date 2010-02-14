using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ISubclassInstance : ISubclassInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ISubclassInstance Not { get; }
        new void DiscriminatorValue(object value);
        new void Abstract();
        new void DynamicInsert();
        new void DynamicUpdate();
        new void LazyLoad();
        new void Proxy(Type type);
        new void Proxy<T>();
        new void SelectBeforeUpdate();
    }
}