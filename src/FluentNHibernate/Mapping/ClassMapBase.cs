using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ClassMapBase<T>
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

        public virtual PropertyMap Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public virtual PropertyMap Map(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var map = new PropertyMap(property, parentIsRequired, typeof(T));

            if (columnName != null)
                map.TheColumnNameIs(columnName);

            _properties.Add(map);

            return map;
        }

        public virtual ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            return References(expression, null);
        }

        public virtual ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression, string columnName)
        {
            var property = ReflectionHelper.GetProperty(expression);
            var part = new ManyToOnePart<OTHER>(property);

            if (columnName != null)
                part.TheColumnNameIs(columnName);

            AddPart(part);

            return part;
        }

        public virtual OneToOnePart<OTHER> HasOne<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            var property = ReflectionHelper.GetProperty(expression);
            var part = new OneToOnePart<OTHER>(property);
            AddPart(part);

            return part;
        }

        public virtual DynamicComponentPart<T> DynamicComponent(Expression<Func<T, IDictionary>> expression, Action<DynamicComponentPart<T>> action)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);

            var part = new DynamicComponentPart<T>(property, parentIsRequired);
            AddPart(part);

            action(part);

            return part;
        }

        public virtual ComponentPart<C> Component<C>(Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);

            ComponentPart<C> part = new ComponentPart<C>(property, parentIsRequired);
            AddPart(part);

            action(part);

            return part;
        }

        /// <summary>
        /// Create a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <typeparam name="RETURN">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        protected virtual OneToManyPart<T, CHILD> MapHasMany<CHILD, RETURN>(Expression<Func<T, RETURN>> expression)
        {
            var part = ReflectionHelper.IsMethodExpression(expression)
                               ? new OneToManyPart<T, CHILD>(ReflectionHelper.GetMethod(expression))
                               : new OneToManyPart<T, CHILD>(ReflectionHelper.GetProperty(expression));

            AddPart(part);

            return part;
        }

        /// <summary>
        /// Create a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, IEnumerable<CHILD>>> expression)
        {
            return MapHasMany<CHILD, IEnumerable<CHILD>>(expression);
        }

        /// <summary>
        /// Create a one-to-many relationship with a IDictionary
        /// </summary>
        /// <typeparam name="KEY">Dictionary key type</typeparam>
        /// <typeparam name="CHILD">Child object type / Dictionary value type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<T, CHILD> HasMany<KEY, CHILD>(Expression<Func<T, IDictionary<KEY, CHILD>>> expression)
        {
            return MapHasMany<CHILD, IDictionary<KEY, CHILD>>(expression);
        }

        /// <summary>
        /// Create a one-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>one-to-many part</returns>
        public OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            return MapHasMany<CHILD, object>(expression);
        }

        /// <summary>
        /// Create a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <typeparam name="RETURN">Property return type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        protected virtual ManyToManyPart<T, CHILD> MapHasManyToMany<CHILD, RETURN>(Expression<Func<T, RETURN>> expression)
        {
            var part = ReflectionHelper.IsMethodExpression(expression)
                               ? new ManyToManyPart<T, CHILD>(ReflectionHelper.GetMethod(expression))
                               : new ManyToManyPart<T, CHILD>(ReflectionHelper.GetProperty(expression));

            AddPart(part);

            return part;
        }

        /// <summary>
        /// Create a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(Expression<Func<T, IEnumerable<CHILD>>> expression)
        {
            return MapHasManyToMany<CHILD, IEnumerable<CHILD>>(expression);
        }

        /// <summary>
        /// Create a many-to-many relationship
        /// </summary>
        /// <typeparam name="CHILD">Child object type</typeparam>
        /// <param name="expression">Expression to get property from</param>
        /// <returns>many-to-many part</returns>
        public ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(Expression<Func<T, object>> expression)
        {
            return MapHasManyToMany<CHILD, object>(expression);
        }

        public virtual VersionPart Version(Expression<Func<T, object>> expression)
        {
            var versionPart = new VersionPart(ReflectionHelper.GetProperty(expression));
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
    }
}
