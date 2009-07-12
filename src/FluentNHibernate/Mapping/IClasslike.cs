using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
    public interface IClasslike
    {
        Type EntityType { get; }
        IEnumerable<PropertyMap> Properties { get; }
        IEnumerable<IComponentBase> Components { get; }

        IProperty Map<TEntity>(Expression<Func<TEntity, object>> expression);
        IManyToOnePart References<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression);
        IOneToOnePart HasOne<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression);
        IComponentBase DynamicComponent<TEntity>(Expression<Func<TEntity, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action);
        IAnyPart<TOther> ReferencesAny<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression);

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        IComponentBase Component<TEntity, TComponent>(Expression<Func<TEntity, TComponent>> expression, Action<ComponentPart<TComponent>> action);

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        IOneToManyPart HasMany<TEntity, TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> expression);

        /// <summary>
        /// CreateProperties a one-to-many relationship with a IDictionary
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TChild">Child object type / Dictionary value type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        IOneToManyPart HasMany<TEntity, TKey, TChild>(Expression<Func<TEntity, IDictionary<TKey, TChild>>> expression);

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        IManyToManyPart HasManyToMany<TEntity, TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> expression);
    }
}