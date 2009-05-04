using System;

namespace FluentNHibernate.Mapping
{
    public interface ISubclass : IClasslike, IMappingPart
    {
        void SubClass<TChild>(object discriminatorValue, Action<ISubclass> action);
        void SubClass<TChild>(Action<ISubclass> action);

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        ISubclass LazyLoad();

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ISubclass Not { get; }
    }
}