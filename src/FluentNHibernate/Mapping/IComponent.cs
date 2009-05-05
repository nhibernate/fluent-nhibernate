using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface IComponent : IClasslike, IMappingPart
    {
        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="exp">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        IComponent WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp);
        ComponentMapping GetComponentMapping();
    }
}