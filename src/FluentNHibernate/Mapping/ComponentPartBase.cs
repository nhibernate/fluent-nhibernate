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
        readonly MappingProviderStore providers;
        readonly AccessStrategyBuilder<TBuilder> access;
        readonly AttributeStore attributes;
        protected bool nextBool = true;

        protected ComponentPartBase(AttributeStore attributes, Member member)
            : this(attributes, member, new MappingProviderStore())
        {}

        protected ComponentPartBase(AttributeStore attributes, Member member, MappingProviderStore providers)
            : base(providers)
        {
            this.attributes = attributes;
            access = new AccessStrategyBuilder<TBuilder>((TBuilder)this, value => attributes.Set("Access", Layer.UserSupplied, value));
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

            attributes.Set("Access", Layer.Defaults, resolvedAccess.ToString());
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
            var parentMapping = new ParentMapping
            {
                ContainingEntityType = typeof(TEntity)
            };
            parentMapping.Set(x => x.Name, Layer.Defaults, property.Name);
            attributes.Set("Parent", Layer.Defaults, parentMapping);

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
            attributes.Set("Insert", Layer.UserSupplied, !nextBool);
            attributes.Set("Update", Layer.UserSupplied, !nextBool);
            nextBool = true;

            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies that this component is insertable.
        /// </summary>
        public TBuilder Insert()
        {
            attributes.Set("Insert", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies that this component is updatable
        /// </summary>
        public TBuilder Update()
        {
            attributes.Set("Update", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the uniqueness of this component
        /// </summary>
        public TBuilder Unique()
        {
            attributes.Set("Unique", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specify that this component should be optimistically locked on access
        /// </summary>
        public TBuilder OptimisticLock()
        {
            attributes.Set("OptimisticLock", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (TBuilder)this;
        }

        protected abstract ComponentMapping CreateComponentMappingRoot(AttributeStore store);

        protected ComponentMapping CreateComponentMapping()
        {
            var mapping = CreateComponentMappingRoot(attributes.Clone());

            if (member != null)
                mapping.Set(x => x.Name, Layer.Defaults, member.Name);

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