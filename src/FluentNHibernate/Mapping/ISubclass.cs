using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface ISubclass : IClasslike
    {
        void Proxy(Type type);
        void Proxy<T>();
        void DynamicUpdate();
        void DynamicInsert();
        void SelectBeforeUpdate();
        void Abstract();

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        void LazyLoad();

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ISubclass Not { get; }

        SubclassMapping GetSubclassMapping();
    }
}