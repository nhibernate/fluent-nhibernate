using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface ISubclass : IClasslike, IMappingPart
    {
        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        ISubclass LazyLoad();

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ISubclass Not { get; }

        SubclassMapping GetSubclassMapping();
    }
}