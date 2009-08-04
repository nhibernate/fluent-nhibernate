using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T>
    {
        private readonly AccessStrategyBuilder<DynamicComponentPart<T>> access;

        public DynamicComponentPart(Type entity, PropertyInfo property)
            : this(new DynamicComponentMapping { ContainingEntityType = entity }, property.Name)
        {}

        private DynamicComponentPart(DynamicComponentMapping mapping, string propertyName)
            : base(mapping, propertyName)
        {
            access = new AccessStrategyBuilder<DynamicComponentPart<T>>(this, value => mapping.Access = value);

            this.mapping = mapping;
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public new AccessStrategyBuilder<DynamicComponentPart<T>> Access
        {
            get { return access; }
        }

        public new DynamicComponentPart<T> ParentReference(Expression<Func<T, object>> exp)
        {
            base.ParentReference(exp);
            return this;
        }

        public new DynamicComponentPart<T> Not
        {
            get
            {
                var forceExecution = base.Not;
                return this;
            }
        }

        public new DynamicComponentPart<T> Unique()
        {
            base.Unique();
            return this;
        }

        public new DynamicComponentPart<T> ReadOnly()
        {
            base.ReadOnly();
            return this;
        }

        public new DynamicComponentPart<T> Insert()
        {
            base.Insert();
            return this;
        }

        public new DynamicComponentPart<T> Update()
        {
            base.Update();
            return this;
        }

        public new DynamicComponentPart<T> OptimisticLock()
        {
            base.OptimisticLock();
            return this;
        }

        public PropertyMap Map(string key)
        {
            return Map<string>(key);
        }

        public PropertyMap Map<TProperty>(string key)
        {
            var property = new DummyPropertyInfo(key, typeof(TProperty));
            var propertyMap = new PropertyMap(property, typeof(T));

            properties.Add(propertyMap);

            return propertyMap;
        }
    }
}