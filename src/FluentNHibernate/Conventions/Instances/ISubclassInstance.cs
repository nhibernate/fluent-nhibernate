using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ISubclassInstance : ISubclassInspector
    {
        ISubclassInstance Not { get; }
        void DiscriminatorValue(object value);
        void Abstract();
        void DynamicInsert();
        void DynamicUpdate();
        void LazyLoad();
        void Proxy(Type type);
        void Proxy<T>();
        void SelectBeforeUpdate();
    }
}