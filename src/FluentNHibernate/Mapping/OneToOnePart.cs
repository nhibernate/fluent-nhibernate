using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class OneToOnePart<TOther> : IOneToOneMappingProvider
    {
        readonly Type entity;
        readonly Member member;
        readonly AccessStrategyBuilder<OneToOnePart<TOther>> access;
        readonly FetchTypeExpression<OneToOnePart<TOther>> fetch;
        readonly CascadeExpression<OneToOnePart<TOther>> cascade;
        readonly AttributeStore attributes = new AttributeStore();
        bool nextBool = true;

        public OneToOnePart(Type entity, Member member)
        {
            access = new AccessStrategyBuilder<OneToOnePart<TOther>>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            fetch = new FetchTypeExpression<OneToOnePart<TOther>>(this, value => attributes.Set("Fetch", Layer.UserSupplied, value));
            cascade = new CascadeExpression<OneToOnePart<TOther>>(this, value => attributes.Set("Cascade", Layer.UserSupplied, value));
            this.entity = entity;
            this.member = member;

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
        /// Specifies the child class
        /// </summary>
        /// <typeparam name="T">Child</typeparam>
        public OneToOnePart<TOther> Class<T>()
        {
            return Class(typeof(T));
        }

        /// <summary>
        /// Specifies the child class
        /// </summary>
        /// <param name="type">Child</param>
        public OneToOnePart<TOther> Class(Type type)
        {
            attributes.Set("Class", Layer.UserSupplied, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Sets the fetch behaviour for this relationship
        /// </summary>
        /// <example>
        /// Fetch.Select();
        /// </example>
        public FetchTypeExpression<OneToOnePart<TOther>> Fetch
        {
            get { return fetch; }
        }

        /// <summary>
        /// Specifies that this relationship should be created with a default-named
        /// foreign-key
        /// </summary>
        public OneToOnePart<TOther> ForeignKey()
        {
            return ForeignKey(string.Format("FK_{0}To{1}", member.DeclaringType.Name, member.Name));
        }

        /// <summary>
        /// Specify the foreign-key constraint name
        /// </summary>
        /// <param name="foreignKeyName">Foreign-key constraint</param>
        public OneToOnePart<TOther> ForeignKey(string foreignKeyName)
        {
            attributes.Set("ForeignKey", Layer.UserSupplied, foreignKeyName);
            return this;
        }

        /// <summary>
        /// Sets the property reference
        /// </summary>
        /// <param name="expression">Property</param>
        public OneToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
        {
            return PropertyRef(expression.ToMember().Name);
        }

        /// <summary>
        /// Sets the property reference
        /// </summary>
        /// <param name="propertyName">Property</param>
        public OneToOnePart<TOther> PropertyRef(string propertyName)
        {
            attributes.Set("PropertyRef", Layer.UserSupplied, propertyName);

            return this;
        }

        /// <summary>
        /// Specifies that this relationship is constrained
        /// </summary>
        public OneToOnePart<TOther> Constrained()
        {
            attributes.Set("Constrained", Layer.UserSupplied, nextBool);
            nextBool = true;

            return this;
        }

        /// <summary>
        /// Sets the cascade behaviour for this relationship
        /// </summary>
        /// <example>
        /// Cascade.All();
        /// </example>
        public CascadeExpression<OneToOnePart<TOther>> Cascade
        {
            get { return cascade; }
        }

        /// <summary>
        /// Specifies the access strategy for this relationship
        /// </summary>
        /// <example>
        /// Access.Field();
        /// </example>
        public AccessStrategyBuilder<OneToOnePart<TOther>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Specify the lazy behaviour of this relationship.
        /// </summary>
        /// <remarks>
        /// Defaults to Proxy lazy-loading. Use the <see cref="Not"/> modifier to disable
        /// lazy-loading, and use the <see cref="LazyLoad(FluentNHibernate.Mapping.Laziness)"/>
        /// overload to specify alternative lazy strategies.
        /// </remarks>
        /// <example>
        /// LazyLoad();
        /// Not.LazyLoad();
        /// </example>
        public OneToOnePart<TOther> LazyLoad()
        {
            if (nextBool)
                LazyLoad(Laziness.Proxy);
            else
                LazyLoad(Laziness.False);

            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the lazy behaviour of this relationship. Cannot be used
        /// with the <see cref="Not"/> modifier.
        /// </summary>
        /// <param name="laziness">Laziness strategy</param>
        /// <example>
        /// LazyLoad(Laziness.NoProxy);
        /// </example>
        public OneToOnePart<TOther> LazyLoad(Laziness laziness)
        {
            attributes.Set("Lazy", Layer.UserSupplied, laziness.ToString());
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public OneToOnePart<TOther> EntityName(string entityName)
        {
            attributes.Set("EntityName", Layer.UserSupplied, entityName);
            return this;
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public OneToOnePart<TOther> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        OneToOneMapping IOneToOneMappingProvider.GetOneToOneMapping()
        {
            var mapping = new OneToOneMapping(attributes.Clone());

            mapping.ContainingEntityType = entity;
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(TOther)));
            mapping.Set(x => x.Name, Layer.Defaults, member.Name);

            return mapping;
        }
    }
}
