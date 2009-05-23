using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ClasslikeMapBase<T> : IClasslike
    {
        protected readonly List<IMappingPart> m_Parts = new List<IMappingPart>();
        public IEnumerable<IMappingPart> Parts
        {
            get { return m_Parts; }
        }
        protected readonly IList<PropertyMap> properties = new List<PropertyMap>();
        protected readonly IList<IComponentBase> components = new List<IComponentBase>();
        protected readonly IList<ISubclass> subclasses = new List<ISubclass>();
        protected readonly IList<IJoinedSubclass> joinedSubclasses = new List<IJoinedSubclass>();
        protected readonly IList<IVersion> versions = new List<IVersion>();

        protected internal void AddPart(IMappingPart part)
        {
            m_Parts.Add(part);
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
            var propertyMapping = new PropertyMapping
            {
                Name = property.Name,
                PropertyInfo = property
            };

            var propertyMap = new PropertyMap(propertyMapping, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

            m_Parts.Add(propertyMap); // backwards compatibility

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
                part.ColumnName(columnName);

            AddPart(part);

            return part;
        }

        public IAnyPart<TOther> ReferencesAny<TOther>(Expression<Func<T, TOther>> expression)
        {
            return ReferencesAny<TOther>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual IAnyPart<TOther> ReferencesAny<TOther>(PropertyInfo property)
        {
            var part = new AnyPart<TOther>(property);
            AddPart(part);
            return part;
        }

        public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, TOther>> expression)
        {
            return HasOne<TOther>(ReflectionHelper.GetProperty(expression));
        }

        protected virtual OneToOnePart<TOther> HasOne<TOther>(PropertyInfo property)
        {
            var part = new OneToOnePart<TOther>(EntityType, property);
            AddPart(part);

            return part;
        }

        public DynamicComponentPart<IDictionary> DynamicComponent(Expression<Func<T, IDictionary>> expression, Action<DynamicComponentPart<IDictionary>> action)
        {
            return DynamicComponent(ReflectionHelper.GetProperty(expression), action);
        }

        public virtual DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property);
            AddPart(part); // old
            action(part);

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
            var part = new ComponentPart<TComponent>(property);
            AddPart(part); // old
            action(part);

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

            AddPart(part);

            return part;
        }

        protected virtual OneToManyPart<TChild> HasMany<TChild>(PropertyInfo property)
        {
            var part = new OneToManyPart<TChild>(EntityType, property);

            AddPart(part);

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

            AddPart(part);

            return part;
        }

        protected virtual ManyToManyPart<TChild> HasManyToMany<TChild>(PropertyInfo property)
        {
            var part = new ManyToManyPart<TChild>(EntityType, property);

            AddPart(part);

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

        public VersionPart Version(Expression<Func<T, object>> expression)
        {
            return Version(ReflectionHelper.GetProperty(expression));
        }

        protected virtual VersionPart Version(PropertyInfo property)
        {
            var versionPart = new VersionPart(EntityType, property);

            versions.Add(versionPart);
            
            return versionPart;
        }

        protected void WriteTheParts(XmlElement classElement, IMappingVisitor visitor)
        {
            m_Parts.Sort(new MappingPartComparer(m_Parts));
            foreach (IMappingPart part in m_Parts)
            {
                part.Write(classElement, visitor);
            }
        }

        IEnumerable<PropertyMap> IClasslike.Properties
        {
            get { return properties; }
        }

        IEnumerable<IComponentBase> IClasslike.Components
        {
            get { return components; }
        }

        IEnumerable<ISubclass> IClasslike.Subclasses
        {
            get { return subclasses; }
        }

        IEnumerable<IJoinedSubclass> IClasslike.JoinedSubclasses
        {
            get { return joinedSubclasses; }
        }

        void IClasslike.AddSubclass(ISubclass subclass)
        {
            subclasses.Add(subclass);
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

        IVersion IClasslike.Version<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Version(ReflectionHelper.GetProperty(expression));
        }

        IProperty IClasslike.Map<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Map(ReflectionHelper.GetProperty(expression), null);
        }

        IManyToOnePart IClasslike.References<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression)
        {
            return References<TOther>(ReflectionHelper.GetProperty(expression), null);
        }

        IAnyPart<TOther> IClasslike.ReferencesAny<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression)
        {
            return ReferencesAny<TOther>(ReflectionHelper.GetProperty(expression));
        }

        IOneToOnePart IClasslike.HasOne<TEntity, TOther>(Expression<Func<TEntity, TOther>> expression)
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
