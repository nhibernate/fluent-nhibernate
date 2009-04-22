using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
    public interface IClasslike
    {
        Type EntityType { get; }
        IEnumerable<IMappingPart> Parts { get; }
        IEnumerable<PropertyMap> Properties { get; }
        IProperty Map<TEntity>(Expression<Func<TEntity, object>> expression);
        IManyToOnePart References<TEntity, OTHER>(Expression<Func<TEntity, OTHER>> expression);
        IOneToOnePart HasOne<TEntity, OTHER>(Expression<Func<TEntity, OTHER>> expression);
        IDynamicComponent DynamicComponent<TEntity>(Expression<Func<TEntity, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action);

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        IComponent Component<TEntity, TComponent>(Expression<Func<TEntity, TComponent>> expression, Action<ComponentPart<TComponent>> action);

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        IOneToManyPart HasMany<TEntity, CHILD>(Expression<Func<TEntity, IEnumerable<CHILD>>> expression);

        /// <summary>
        /// CreateProperties a one-to-many relationship with a IDictionary
        /// </summary>
        /// <typeparam name="KEY">Dictionary key type</typeparam>
        /// <typeparam name="CHILD">Child object type / Dictionary value type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        IOneToManyPart HasMany<TEntity, KEY, CHILD>(Expression<Func<TEntity, IDictionary<KEY, CHILD>>> expression);

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        IManyToManyPart HasManyToMany<TEntity, CHILD>(Expression<Func<TEntity, IEnumerable<CHILD>>> expression);

        IVersion Version<T>(Expression<Func<T, object>> expression);
    }
}