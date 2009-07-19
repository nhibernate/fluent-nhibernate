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
        protected readonly IList<PropertyMap> properties = new List<PropertyMap>();
        protected readonly IList<IComponentBase> components = new List<IComponentBase>();
        protected readonly IList<IOneToOneMappingProvider> oneToOnes = new List<IOneToOneMappingProvider>();
        protected readonly Dictionary<Type, ISubclassMappingProvider> subclasses = new Dictionary<Type, ISubclassMappingProvider>();
        protected readonly Dictionary<Type, IJoinedSubclassMappingProvider> joinedSubclasses = new Dictionary<Type, IJoinedSubclassMappingProvider>();
        protected readonly IList<ICollectionRelationship> collections = new List<ICollectionRelationship>();
        protected readonly IList<IManyToOnePart> references = new List<IManyToOnePart>();
        protected readonly IList<IAnyMappingProvider> anys = new List<IAnyMappingProvider>();

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
            var propertyMap = new PropertyMap(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

            properties.Add(propertyMap);

            return propertyMap;
        }

        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression)
        {
            return References(expression, null);
        }

        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> expression, string columnName)
        {
            return References<TOther>(ReflectionHelper.GetProperty(expression), columnName);
        }

        protected virtual ManyToOnePart<TOther> References<TOther>(PropertyInfo property, string columnName)
        {
            var part = new ManyToOnePart<TOther>(EntityType, property);

            if (columnName != null)
                part.Column(columnName);

            references.Add(part);

            return part;
        }

        public AnyPart<TOther> ReferencesAny<TOther>(Expression<Func<T, TOther>> expression)
        {
            return ReferencesAny<TOther>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual AnyPart<TOther> ReferencesAny<TOther>(PropertyInfo property)
        {
            var part = new AnyPart<TOther>(typeof(T), property);

            anys.Add(part);

            return part;
        }

        public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, TOther>> expression)
        {
            return HasOne<TOther>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual OneToOnePart<TOther> HasOne<TOther>(PropertyInfo property)
        {
            var part = new OneToOnePart<TOther>(EntityType, property);

            oneToOnes.Add(part);

            return part;
        }

        public DynamicComponentPart<IDictionary> DynamicComponent(Expression<Func<T, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action)
        {
            return DynamicComponent(ReflectionHelper.GetProperty(expression), action);
        }

        protected DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(typeof(T), property);
            
            action(part);

            components.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> expression, Action<ComponentPart<TComponent>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, object>> expression, Action<ComponentPart<TComponent>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }
        
        protected virtual ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(typeof(T), property);

            action(part);

            components.Add(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <typeparam name="TReturn">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        private OneToManyPart<TChild> MapHasMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return ReflectionHelper.IsMethodExpression(expression)
                               ? HasMany<TChild>(ReflectionHelper.GetMethod(expression))
                               : HasMany<TChild>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual OneToManyPart<TChild> HasMany<TChild>(MethodInfo method)
        {
            var part = new OneToManyPart<TChild>(EntityType, method);

            collections.Add(part);

            return part;
        }

        protected virtual OneToManyPart<TChild> HasMany<TChild>(PropertyInfo property)
        {
            var part = new OneToManyPart<TChild>(EntityType, property);

            collections.Add(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> expression)
        {
            return MapHasMany<TChild, IEnumerable<TChild>>(expression);
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship with a IDictionary
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TChild">Child object type / Dictionary value type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<TChild> HasMany<TKey, TChild>(Expression<Func<T, IDictionary<TKey, TChild>>> expression)
        {
            return MapHasMany<TChild, IDictionary<TKey, TChild>>(expression);
        }

        /// <summary>
        /// CreateProperties a one-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, object>> expression)
        {
            return MapHasMany<TChild, object>(expression);
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <typeparam name="TReturn">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        private ManyToManyPart<TChild> MapHasManyToMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return ReflectionHelper.IsMethodExpression(expression)
                               ? HasManyToMany<TChild>(ReflectionHelper.GetMethod(expression))
                               : HasManyToMany<TChild>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual ManyToManyPart<TChild> HasManyToMany<TChild>(MethodInfo method)
        {
            var part = new ManyToManyPart<TChild>(EntityType, method);

            collections.Add(part);

            return part;
        }

        protected virtual ManyToManyPart<TChild> HasManyToMany<TChild>(PropertyInfo property)
        {
            var part = new ManyToManyPart<TChild>(EntityType, property);

            collections.Add(part);

            return part;
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> expression)
        {
            return MapHasManyToMany<TChild, IEnumerable<TChild>>(expression);
        }

        /// <summary>
        /// CreateProperties a many-to-many relationship
        /// </summary>
        /// <typeparam name="TChild">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, object>> expression)
        {
            return MapHasManyToMany<TChild, object>(expression);
        }

        IEnumerable<PropertyMap> IClasslike.Properties
        {
			get { return Properties; }
        }

		protected virtual IEnumerable<PropertyMap> Properties
		{
			get { return properties; }
		}

        IEnumerable<IComponentBase> IClasslike.Components
        {
            get { return Components; }
        }

		protected virtual IEnumerable<IComponentBase> Components
		{
			get { return components; }
		}

        public Type EntityType
        {
            get { return typeof(T); }
        }

        #region Explicit IClasslike implementation

        IComponentBase IClasslike.DynamicComponent<TEntity>(Expression<Func<TEntity, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action)
        {
            return DynamicComponent(ReflectionHelper.GetProperty(expression), action);
        }

        IComponentBase IClasslike.Component<TEntity, TComponent>(Expression<Func<TEntity, TComponent>> expression, Action<ComponentPart<TComponent>> action)
        {
            return Component(ReflectionHelper.GetProperty(expression), action);
        }

        IProperty IClasslike.Map<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Map(ReflectionHelper.GetProperty(expression), null);
        }

        IManyToOnePart IClasslike.References<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression)
        {
            return References<TOther>(ReflectionHelper.GetProperty(expression), null);
        }

        IOneToOneMappingProvider IClasslike.HasOne<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression)
        {
            return HasOne<TOther>(ReflectionHelper.GetProperty(expression));
        }

        IOneToManyPart IClasslike.HasMany<TEntity, TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> expression)
        {
            return HasMany<TChild>(ReflectionHelper.GetProperty(expression));
        }

        IOneToManyPart IClasslike.HasMany<TEntity, TKey, TChild>(Expression<Func<TEntity, IDictionary<TKey, TChild>>> expression)
        {
            return HasMany<TChild>(ReflectionHelper.GetProperty(expression));
        }

        IManyToManyPart IClasslike.HasManyToMany<TEntity, TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> expression)
        {
            return HasManyToMany<TChild>(ReflectionHelper.GetProperty(expression));
        }

        #endregion
    }
}
