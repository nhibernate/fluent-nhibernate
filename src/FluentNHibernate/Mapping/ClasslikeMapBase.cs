using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ClasslikeMapBase<T>
    {
        readonly MappingProviderStore providers;

        protected ClasslikeMapBase(MappingProviderStore providers)
        {
            this.providers = providers;
        }

        /// <summary>
        /// Create a property mapping.
        /// </summary>
        /// <param name="memberExpression">Property to map</param>
        /// <example>
        /// Map(x => x.Name);
        /// </example>
        public PropertyPart Map(Expression<Func<T, object>> memberExpression)
        {
            return Map(memberExpression, null);
        }

        /// <summary>
        /// Create a property mapping.
        /// </summary>
        /// <param name="memberExpression">Property to map</param>
        /// <param name="columnName">Property column name</param>
        /// <example>
        /// Map(x => x.Name, "person_name");
        /// </example>
        public PropertyPart Map(Expression<Func<T, object>> memberExpression, string columnName)
        {
#pragma warning disable 612,618
            return Map(memberExpression.ToMember(), columnName);
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual PropertyPart Map(Member property, string columnName)
        {
            var propertyMap = new PropertyPart(property, typeof(T));

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.Column(columnName);

            providers.Properties.Add(propertyMap);

            return propertyMap;
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property on the current entity</param>
        /// <example>
        /// References(x => x.Company);
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> memberExpression)
        {
            return References(memberExpression, null);
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property on the current entity</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// References(x => x.Company, "company_id");
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> memberExpression, string columnName)
        {
#pragma warning disable 612,618
            return References<TOther>(memberExpression.ToMember(), columnName);
#pragma warning restore 612,618
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property on the current entity</param>
        /// <example>
        /// References(x => x.Company, "company_id");
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, object>> memberExpression)
        {
            return References<TOther>(memberExpression, null);
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property on the current entity</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// References(x => x.Company, "company_id");
        /// </example>
        public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, object>> memberExpression, string columnName)
        {
#pragma warning disable 612,618
            return References<TOther>(memberExpression.ToMember(), columnName);
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            var part = new ManyToOnePart<TOther>(EntityType, property);

            if (columnName != null)
                part.Column(columnName);

            providers.References.Add(part);

            return part;
        }

        /// <summary>
        /// Create a reference to any other entity. This is an "any" polymorphic relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity to reference</typeparam>
        /// <param name="memberExpression">Property</param>
        public AnyPart<TOther> ReferencesAny<TOther>(Expression<Func<T, TOther>> memberExpression)
        {
#pragma warning disable 612,618
            return ReferencesAny<TOther>(memberExpression.ToMember());
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual AnyPart<TOther> ReferencesAny<TOther>(Member property)
        {
            var part = new AnyPart<TOther>(typeof(T), property);

            providers.Anys.Add(part);

            return part;
        }

        /// <summary>
        /// Create a reference to another entity based exclusively on the primary-key values.
        /// This is sometimes called a one-to-one relationship, in database terms. Generally
        /// you should use <see cref="References{TOther}(System.Linq.Expressions.Expression{System.Func{T,object}})"/>
        /// whenever possible.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property</param>
        /// <example>
        /// HasOne(x => x.ExtendedInfo);
        /// </example>
        public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, Object>> memberExpression)
        {
#pragma warning disable 612,618
            return HasOne<TOther>(memberExpression.ToMember());
#pragma warning restore 612,618
        }

        /// <summary>
        /// Create a reference to another entity based exclusively on the primary-key values.
        /// This is sometimes called a one-to-one relationship, in database terms. Generally
        /// you should use <see cref="References{TOther}(System.Linq.Expressions.Expression{System.Func{T,object}})"/>
        /// whenever possible.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="memberExpression">Property</param>
        /// <example>
        /// HasOne(x => x.ExtendedInfo);
        /// </example>
        public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, TOther>> memberExpression)
        {
#pragma warning disable 612,618
            return HasOne<TOther>(memberExpression.ToMember());
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual OneToOnePart<TOther> HasOne<TOther>(Member property)
        {
            var part = new OneToOnePart<TOther>(EntityType, property);

            providers.OneToOnes.Add(part);

            return part;
        }

        /// <summary>
        /// Create a dynamic component mapping. This is a dictionary that represents
        /// a limited number of columns in the database.
        /// </summary>
        /// <param name="memberExpression">Property containing component</param>
        /// <param name="dynamicComponentAction">Component setup action</param>
        /// <example>
        /// DynamicComponent(x => x.Data, comp =>
        /// {
        ///   comp.Map(x => (int)x["age"]);
        /// });
        /// </example>
        public DynamicComponentPart<IDictionary> DynamicComponent(Expression<Func<T, IDictionary>> memberExpression, Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
        {
#pragma warning disable 612,618
            return DynamicComponent(memberExpression.ToMember(), dynamicComponentAction);
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected DynamicComponentPart<IDictionary> DynamicComponent(Member property, Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
        {
            var part = new DynamicComponentPart<IDictionary>(typeof(T), property);
            
            dynamicComponentAction(part);

            providers.Components.Add(part);

            return part;
        }

        /// <summary>
        /// Creates a component reference. This is a place-holder for a component that is defined externally with a
        /// <see cref="ComponentMap{T}"/>; the mapping defined in said <see cref="ComponentMap{T}"/> will be merged
        /// with any options you specify from this call.
        /// </summary>
        /// <typeparam name="TComponent">Component type</typeparam>
        /// <param name="member">Property exposing the component</param>
        /// <returns>Component reference builder</returns>
        public virtual ReferenceComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> member)
        {
            var part = new ReferenceComponentPart<TComponent>(member.ToMember(), typeof(T));

            providers.Components.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        /// <example>
        /// Component(x => x.Address, comp =>
        /// {
        ///   comp.Map(x => x.Street);
        ///   comp.Map(x => x.City);
        /// });
        /// </example>
        public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> expression, Action<ComponentPart<TComponent>> action)
        {
#pragma warning disable 612,618
            return Component(expression.ToMember(), action);
#pragma warning restore 612,618
        }

        /// <summary>
        /// Maps a component
        /// </summary>
        /// <typeparam name="TComponent">Type of component</typeparam>
        /// <param name="expression">Component property</param>
        /// <param name="action">Component mapping</param>
        /// <example>
        /// Component(x => x.Address, comp =>
        /// {
        ///   comp.Map(x => x.Street);
        ///   comp.Map(x => x.City);
        /// });
        /// </example>
        public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, object>> expression, Action<ComponentPart<TComponent>> action)
        {
#pragma warning disable 612,618
            return Component(expression.ToMember(), action);
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual ComponentPart<TComponent> Component<TComponent>(Member property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(typeof(T), property);

            action(part);

            providers.Components.Add(part);

            return part;
        }

        /// <summary>
        /// Allows the user to add a custom component mapping to the class mapping.        
        /// Note: not a fluent method.
        /// </summary>
        /// <remarks>
        /// In some cases, our users need a way to add an instance of their own implementation of IComponentMappingProvider.
        /// For an example of where this might be necessary, see: http://codebetter.com/blogs/jeremy.miller/archive/2010/02/16/our-extension-properties-story.aspx
        /// </remarks>        
        public void Component(IComponentMappingProvider componentProvider)
        {
            providers.Components.Add(componentProvider);
        }

        private OneToManyPart<TChild> MapHasMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
        {
#pragma warning disable 612,618
            return HasMany<TChild>(expression.ToMember());
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual OneToManyPart<TChild> HasMany<TChild>(Member member)
        {
            var part = new OneToManyPart<TChild>(EntityType, member);

            providers.Collections.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a collection of entities as a one-to-many
        /// </summary>
        /// <typeparam name="TChild">Child entity type</typeparam>
        /// <param name="memberExpression">Collection property</param>
        /// <example>
        /// HasMany(x => x.Locations);
        /// </example>
        public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression)
        {
            return MapHasMany<TChild, IEnumerable<TChild>>(memberExpression);
        }

        public OneToManyPart<TChild> HasMany<TKey, TChild>(Expression<Func<T, IDictionary<TKey, TChild>>> memberExpression)
        {
            return MapHasMany<TChild, IDictionary<TKey, TChild>>(memberExpression);
        }

        /// <summary>
        /// Maps a collection of entities as a one-to-many
        /// </summary>
        /// <typeparam name="TChild">Child entity type</typeparam>
        /// <param name="memberExpression">Collection property</param>
        /// <example>
        /// HasMany(x => x.Locations);
        /// </example>
        public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, object>> memberExpression)
        {
            return MapHasMany<TChild, object>(memberExpression);
        }

        private ManyToManyPart<TChild> MapHasManyToMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
        {
#pragma warning disable 612,618
            return HasManyToMany<TChild>(expression.ToMember());
#pragma warning restore 612,618
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual ManyToManyPart<TChild> HasManyToMany<TChild>(Member property)
        {
            var part = new ManyToManyPart<TChild>(EntityType, property);

            providers.Collections.Add(part);

            return part;
        }

        /// <summary>
        /// Maps a collection of entities as a many-to-many
        /// </summary>
        /// <typeparam name="TChild">Child entity type</typeparam>
        /// <param name="memberExpression">Collection property</param>
        /// <example>
        /// HasManyToMany(x => x.Locations);
        /// </example>
        public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression)
        {
            return MapHasManyToMany<TChild, IEnumerable<TChild>>(memberExpression);
        }

        /// <summary>
        /// Maps a collection of entities as a many-to-many
        /// </summary>
        /// <typeparam name="TChild">Child entity type</typeparam>
        /// <param name="memberExpression">Collection property</param>
        /// <example>
        /// HasManyToMany(x => x.Locations);
        /// </example>
        public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, object>> memberExpression)
        {
            return MapHasManyToMany<TChild, object>(memberExpression);
        }

        /// <summary>
        /// Specify an insert stored procedure
        /// </summary>
        /// <param name="innerText">Stored procedure call</param>
        public StoredProcedurePart SqlInsert(string innerText)
        {
            return StoredProcedure("sql-insert", innerText);
        }

        /// <summary>
        /// Specify an update stored procedure
        /// </summary>
        /// <param name="innerText">Stored procedure call</param>
        public StoredProcedurePart SqlUpdate(string innerText)
        {
            return StoredProcedure("sql-update", innerText);
        }

        /// <summary>
        /// Specify an delete stored procedure
        /// </summary>
        /// <param name="innerText">Stored procedure call</param>
        public StoredProcedurePart SqlDelete(string innerText)
        {
            return StoredProcedure("sql-delete", innerText);
        }

        /// <summary>
        /// Specify an delete all stored procedure
        /// </summary>
        /// <param name="innerText">Stored procedure call</param>
        public StoredProcedurePart SqlDeleteAll(string innerText)
        {
            return StoredProcedure("sql-delete-all", innerText);
        }

        protected StoredProcedurePart StoredProcedure(string element, string innerText)
        {
            var part = new StoredProcedurePart(element, innerText);
            providers.StoredProcedures.Add(part);
            return part;
        }

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual IEnumerable<IPropertyMappingProvider> Properties
		{
            get { return providers.Properties; }
		}

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        protected virtual IEnumerable<IComponentMappingProvider> Components
		{
            get { return providers.Components; }
		}

        [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
        public Type EntityType
        {
            get { return typeof(T); }
        }
    }
}
