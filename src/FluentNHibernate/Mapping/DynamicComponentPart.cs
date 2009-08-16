using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DynamicComponentPart<T> : ComponentPartBase<T>
    {
        private readonly Type entity;
        private readonly AccessStrategyBuilder<DynamicComponentPart<T>> access;
        private readonly AttributeStore<DynamicComponentMapping> attributes;

        public DynamicComponentPart(Type entity, PropertyInfo property)
            : this(entity, property.Name, new AttributeStore())
        {}

        private DynamicComponentPart(Type entity, string propertyName, AttributeStore underlyingStore)
            : base(underlyingStore, propertyName)
        {
            this.entity = entity;
            attributes = new AttributeStore<DynamicComponentMapping>(underlyingStore);
            access = new AccessStrategyBuilder<DynamicComponentPart<T>>(this, value => attributes.Set(x => x.Access, value));
        }

        protected override IComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            return new DynamicComponentMapping(store)
            {
                ContainingEntityType = entity
            };
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

        public PropertyPart Map(string key)
        {
            return Map<string>(key);
        }

        public PropertyPart Map<TProperty>(string key)
        {
            var property = new DummyPropertyInfo(key, typeof(TProperty));
            var propertyMap = new PropertyPart(property, typeof(T));

            properties.Add(propertyMap);

            return propertyMap;
        }
    }
}