using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ComponentPartBase<T>
    {
        private readonly Type entity;
        private readonly AccessStrategyBuilder<ComponentPart<T>> access;
        private readonly AttributeStore<ComponentMapping> attributes;

        public ComponentPart(Type entity, PropertyInfo property)
            : this(entity, property.Name, new AttributeStore())
        {}

        private ComponentPart(Type entity, string propertyName, AttributeStore underlyingStore)
            : base(underlyingStore, propertyName)
        {
            attributes = new AttributeStore<ComponentMapping>(underlyingStore);
            access = new AccessStrategyBuilder<ComponentPart<T>>(this, value => attributes.Set(x => x.Access, value));
            this.entity = entity;

            Insert();
            Update();
        }

        protected override IComponentMapping CreateComponentMappingRoot(AttributeStore store)
        {
            return new ComponentMapping(store)
            {
                ContainingEntityType = entity
            };
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public new AccessStrategyBuilder<ComponentPart<T>> Access
        {
            get { return access; }
        }

        public new ComponentPart<T> ParentReference(Expression<Func<T, object>> exp)
        {
            base.ParentReference(exp);
            return this;
        }

        public new ComponentPart<T> Not
        {
            get
            {
                var forceExecution = base.Not;
                return this;
            }
        }

        public new ComponentPart<T> ReadOnly()
        {
            base.ReadOnly();
            return this;
        }

        public new ComponentPart<T> Insert()
        {
            base.Insert();
            return this;
        }

        public new ComponentPart<T> Update()
        {
            base.Update();
            return this;
        }

        public ComponentPart<T> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public new ComponentPart<T> OptimisticLock()
        {
            base.OptimisticLock();
            return this;
        }
    }
}
