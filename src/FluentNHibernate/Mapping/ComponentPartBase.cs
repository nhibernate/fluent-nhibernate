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
        readonly Member member;
        private readonly MappingProviderStore providers;
        readonly AccessStrategyBuilder<TBuilder> access;
        readonly AttributeStore<ComponentMappingBase> attributes;
        protected bool nextBool = true;

        protected ComponentPartBase(AttributeStore underlyingStore, Member member)
            : this(underlyingStore, member, new MappingProviderStore())
        {}

        protected ComponentPartBase(AttributeStore underlyingStore, Member member, MappingProviderStore providers)
            : base(providers)
        {
            attributes = new AttributeStore<ComponentMappingBase>(underlyingStore);
            access = new AccessStrategyBuilder<TBuilder>((TBuilder)this, value => attributes.Set(x => x.Access, value));
            this.member = member;
            this.providers = providers;

            if (member != null)
                SetDefaultAccess();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.SetDefault(x => x.Access, resolvedAccess.ToString());
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

            if (member != null)
                mapping.Name = member.Name;

            foreach (var property in providers.Properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in providers.Components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var oneToOne in providers.OneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in providers.Collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in providers.References)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in providers.Anys)
                mapping.AddAny(any.GetAnyMapping());

            return mapping;
        }
    }
}