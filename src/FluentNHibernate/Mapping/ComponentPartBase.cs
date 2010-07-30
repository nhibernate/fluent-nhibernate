using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ComponentPartBase<TEntity, TBuilder> : ClasslikeMapBase<TEntity>
        where TBuilder : ComponentPartBase<TEntity, TBuilder>
    {
        readonly string propertyName;
        readonly AccessStrategyBuilder<TBuilder> access;
        readonly AttributeStore<ComponentMappingBase> attributes;
        protected bool nextBool = true;

        protected ComponentPartBase(AttributeStore underlyingStore, string propertyName)
        {
            attributes = new AttributeStore<ComponentMappingBase>(underlyingStore);
            access = new AccessStrategyBuilder<TBuilder>((TBuilder)this, value => attributes.Set(x => x.Access, value));
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<TBuilder> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Specify a parent reference for this component
        /// </summary>
        /// <param name="expression">Parent property</param>
        /// <example>
        /// ParentReference(x => x.Parent);
        /// </example>
        public TBuilder ParentReference(Expression<Func<TEntity, object>> expression)
        {
            return ParentReference(expression.ToMember());
        }

        private TBuilder ParentReference(Member property)
        {
            attributes.Set(x => x.Parent, new ParentMapping
            {
                Name = property.Name,
                ContainingEntityType = typeof(TEntity)
            });

            return (TBuilder)this;
        }

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TBuilder Not
        {
            get
            {
                nextBool = !nextBool;
                return (TBuilder)this;
            }
        }

        /// <summary>
        /// Specifies that this component is read-only
        /// </summary>
        /// <remarks>
        /// This is the same as calling both Not.Insert() and Not.Update()
        /// </remarks>
        public TBuilder ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            attributes.Set(x => x.Update, !nextBool);
            nextBool = true;

            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies that this component is insertable.
        /// </summary>
        public TBuilder Insert()
        {
            attributes.Set(x => x.Insert, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies that this component is updatable
        /// </summary>
        public TBuilder Update()
        {
            attributes.Set(x => x.Update, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the uniqueness of this component
        /// </summary>
        public TBuilder Unique()
        {
            attributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specify that this component should be optimistically locked on access
        /// </summary>
        public TBuilder OptimisticLock()
        {
            attributes.Set(x => x.OptimisticLock, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        protected abstract ComponentMapping CreateComponentMappingRoot(AttributeStore store);

        protected ComponentMapping CreateComponentMapping()
        {
            var mapping = CreateComponentMappingRoot(attributes.CloneInner());

            mapping.Name = propertyName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var oneToOne in oneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            return mapping;
        }
    }
}