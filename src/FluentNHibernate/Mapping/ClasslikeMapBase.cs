using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ClasslikeMapBase<T> : IClasslike
    {
        private bool _parentIsRequired = true;
        private readonly List<IMappingPart> _properties = new List<IMappingPart>();

        protected bool parentIsRequired
        {
            get { return _parentIsRequired; }
            set { _parentIsRequired = value; }
        }

        protected internal void AddPart(IMappingPart part)
        {
            _properties.Add(part);
        }

        public PropertyMap Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public PropertyMap Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual PropertyMap Map(PropertyInfo property, string columnName)
        {
            var map = new PropertyMap(property, parentIsRequired, EntityType);

            if (columnName != null)
                map.ColumnNames.Add(columnName);

            _properties.Add(map);

            return map;
        }

        public ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            return References(expression, null);
        }

        public ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression, string columnName)
        {
            return References<OTHER>(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual ManyToOnePart<OTHER> References<OTHER>(PropertyInfo property, string columnName)
        {
            var part = new ManyToOnePart<OTHER>(EntityType, property);

            if (columnName != null)
                part.ColumnName(columnName);

            AddPart(part);

            return part;
        }

        public OneToOnePart<OTHER> HasOne<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            return HasOne<OTHER>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual OneToOnePart<OTHER> HasOne<OTHER>(PropertyInfo property)
        {
            var part = new OneToOnePart<OTHER>(EntityType, property);
            AddPart(part);

            return part;
        }

        public IDynamicComponent DynamicComponent(Expression<Func<T, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action)
        {
            return DynamicComponent(ReflectionHelper.GetProperty(expression), action);
        }

        public virtual IDynamicComponent DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property, parentIsRequired);
            AddPart(part);

            action(part);

            return part;
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="C">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        public ComponentPart<C> Component<C>(Expression<Func<T, C>> expression, Action<ComponentPart<C>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="C">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        public ComponentPart<C> Component<C>(Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }

        protected virtual ComponentPart<C> Component<C>(PropertyInfo property, Action<ComponentPart<C>> action)
        {
            var part = new ComponentPart<C>(property, parentIsRequired);
            AddPart(part);

            action(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <typeparam name="RETURN">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        private OneToManyPart<CHILD> MapHasMany<CHILD, RETURN>(Expression<Func<T, RETURN>> expression)
        {
            return ReflectionHelper.IsMethodExpression(expression)
                               ? HasMany<CHILD>(ReflectionHelper.GetMethod(expression))
                               : HasMany<CHILD>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual OneToManyPart<CHILD> HasMany<CHILD>(MethodInfo method)
        {
            var part = new OneToManyPart<CHILD>(EntityType, method);

            AddPart(part);

            return part;
        }

        protected virtual OneToManyPart<CHILD> HasMany<CHILD>(PropertyInfo property)
        {
            var part = new OneToManyPart<CHILD>(EntityType, property);

            AddPart(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<CHILD> HasMany<CHILD>(Expression<Func<T, IEnumerable<CHILD>>> expression)
        {
            return MapHasMany<CHILD, IEnumerable<CHILD>>(expression);
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship with a IDictionary
        /// </summary>
        /// <typeparam name="KEY">Dictionary key type</typeparam>
        /// <typeparam name="CHILD">Child object type / Dictionary value type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<CHILD> HasMany<KEY, CHILD>(Expression<Func<T, IDictionary<KEY, CHILD>>> expression)
        {
            return MapHasMany<CHILD, IDictionary<KEY, CHILD>>(expression);
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            return MapHasMany<CHILD, object>(expression);
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <typeparam name="RETURN">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        private ManyToManyPart<CHILD> MapHasManyToMany<CHILD, RETURN>(Expression<Func<T, RETURN>> expression)
        {
            return ReflectionHelper.IsMethodExpression(expression)
                               ? HasManyToMany<CHILD>(ReflectionHelper.GetMethod(expression))
                               : HasManyToMany<CHILD>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual ManyToManyPart<CHILD> HasManyToMany<CHILD>(MethodInfo method)
        {
            var part = new ManyToManyPart<CHILD>(EntityType, method);

            AddPart(part);

            return part;
        }

        protected virtual ManyToManyPart<CHILD> HasManyToMany<CHILD>(PropertyInfo property)
        {
            var part = new ManyToManyPart<CHILD>(EntityType, property);

            AddPart(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<CHILD> HasManyToMany<CHILD>(Expression<Func<T, IEnumerable<CHILD>>> expression)
        {
            return MapHasManyToMany<CHILD, IEnumerable<CHILD>>(expression);
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<CHILD> HasManyToMany<CHILD>(Expression<Func<T, object>> expression)
        {
            return MapHasManyToMany<CHILD, object>(expression);
        }

        public VersionPart Version(Expression<Func<T, object>> expression)
        {
            return Version(ReflectionHelper.GetProperty(expression));
        }

        protected virtual VersionPart Version(PropertyInfo property)
        {
            var versionPart = new VersionPart(EntityType, property);
            AddPart(versionPart);
            return versionPart;
        }

        protected void writeTheParts(XmlElement classElement, IMappingVisitor visitor)
        {
            _properties.Sort(new MappingPartComparer());
            foreach (IMappingPart part in _properties)
            {
                part.Write(classElement, visitor);
            }
        }

        public IEnumerable<IMappingPart> Parts
        {
            get { return _properties; }
        }

        public Type EntityType
        {
            get { return typeof(T); }
        }

        #region Explicit IClasslike implementation

        IDynamicComponent IClasslike.DynamicComponent<TEntity>(Expression<Func<TEntity, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action)
        {
            return DynamicComponent(ReflectionHelper.GetProperty(expression), action);
        }

        IVersion IClasslike.Version<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Version(ReflectionHelper.GetProperty(expression));
        }

        IProperty IClasslike.Map<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Map(ReflectionHelper.GetProperty(expression), null);
        }

        IManyToOnePart IClasslike.References<TEntity, OTHER>(Expression<Func<TEntity, OTHER>> expression)
        {
            return References<OTHER>(ReflectionHelper.GetProperty(expression), null);
        }

        IOneToOnePart IClasslike.HasOne<TEntity, OTHER>(Expression<Func<TEntity, OTHER>> expression)
        {
            return HasOne<OTHER>(ReflectionHelper.GetProperty(expression));
        }

        IComponent IClasslike.Component<TEntity, TComponent>(Expression<Func<TEntity, TComponent>> expression, Action<ComponentPart<TComponent>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }

        IOneToManyPart IClasslike.HasMany<TEntity, CHILD>(Expression<Func<TEntity, IEnumerable<CHILD>>> expression)
        {
            return HasMany<CHILD>(ReflectionHelper.GetProperty(expression));
        }

        IOneToManyPart IClasslike.HasMany<TEntity, KEY, CHILD>(Expression<Func<TEntity, IDictionary<KEY, CHILD>>> expression)
        {
            return HasMany<CHILD>(ReflectionHelper.GetProperty(expression));
        }

        IManyToManyPart IClasslike.HasManyToMany<TEntity, CHILD>(Expression<Func<TEntity, IEnumerable<CHILD>>> expression)
        {
            return HasManyToMany<CHILD>(ReflectionHelper.GetProperty(expression));
        }

        #endregion
    }
}
