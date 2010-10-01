using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping.Builders
{
    /// <summary>
    /// Component-element for component HasMany's.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class CompositeElementBuilder<T>
    {
        readonly CompositeElementMapping mapping;

        public CompositeElementBuilder(CompositeElementMapping mapping, Type containingEntityType)
        {
            this.mapping = mapping;

            InitialiseDefaults(containingEntityType);
        }

        public CompositeElementBuilder(CompositeElementMapping mapping, Type containingEntityType, Member member)
            : this(mapping, containingEntityType)
        {
            InitialiseDefaults(member);
        }

        void InitialiseDefaults(Type containingEntityType)
        {
            mapping.ContainingEntityType = containingEntityType;

            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(typeof(T));
        }

        void InitialiseDefaults(Member member)
        {
            mapping.As<NestedCompositeElementMapping>(ce =>
                ce.Name = member.Name);
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="expression">Property</param>
        /// <example>
        /// Map(x => x.Age);
        /// </example>
        public PropertyBuilder Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        /// <summary>
        /// Map a property
        /// </summary>
        /// <param name="expression">Property</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// Map(x => x.Age, "person_age");
        /// </example>
        public PropertyBuilder Map(Expression<Func<T, object>> expression, string columnName)
        {
            return Map(expression.ToMember(), columnName);
        }

        protected virtual PropertyBuilder Map(Member property, string columnName)
        {
            var propertyMapping = new PropertyMapping();
            var builder = new PropertyBuilder(propertyMapping, typeof(T), property);

            if (!string.IsNullOrEmpty(columnName))
                builder.Column(columnName);

            mapping.AddProperty(propertyMapping);

            return builder;
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="expression">Property on the current entity</param>
        /// <example>
        /// References(x => x.Company);
        /// </example>
        public ManyToOneBuilder<TOther> References<TOther>(Expression<Func<T, TOther>> expression)
        {
            return References(expression, null);
        }

        /// <summary>
        /// Create a reference to another entity. In database terms, this is a many-to-one
        /// relationship.
        /// </summary>
        /// <typeparam name="TOther">Other entity</typeparam>
        /// <param name="expression">Property on the current entity</param>
        /// <param name="columnName">Column name</param>
        /// <example>
        /// References(x => x.Company, "person_company_id");
        /// </example>
        public ManyToOneBuilder<TOther> References<TOther>(Expression<Func<T, TOther>> expression, string columnName)
        {
            return References<TOther>(expression.ToMember(), columnName);
        }

        protected virtual ManyToOneBuilder<TOther> References<TOther>(Member property, string columnName)
        {
            var manyToOneMapping = new ManyToOneMapping();
            var part = new ManyToOneBuilder<TOther>(manyToOneMapping, typeof(T), property);

            if (columnName != null)
                part.Column(columnName);

            mapping.AddReference(manyToOneMapping);

            return part;
        }

        /// <summary>
        /// Maps a property of the component class as a reference back to the containing entity
        /// </summary>
        /// <param name="expression">Parent reference property</param>
        /// <returns>Component being mapped</returns>
        public void ParentReference(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();
            mapping.Parent = new ParentMapping
            {
                Name = member.Name,
                ContainingEntityType = mapping.ContainingEntityType
            };
        }

        /// <summary>
        /// Create a nested component mapping.
        /// </summary>
        /// <param name="property">Component property</param>
        /// <param name="nestedCompositeElementAction">Action for creating the component</param>
        /// <example>
        /// HasMany(x => x.Locations)
        ///   .Component(c =>
        ///   {
        ///     c.Map(x => x.Name);
        ///     c.Component(x => x.Address, addr =>
        ///     {
        ///       addr.Map(x => x.Street);
        ///       addr.Map(x => x.PostCode);
        ///     });
        ///   });
        /// </example>
        public void Component<TChild>(Expression<Func<T, TChild>> property, Action<CompositeElementBuilder<TChild>> nestedCompositeElementAction)
        {
            var nestedMapping = new NestedCompositeElementMapping();
            var nestedCompositeElement = new CompositeElementBuilder<TChild>(nestedMapping, mapping.ContainingEntityType, property.ToMember());

            nestedCompositeElementAction(nestedCompositeElement);

            mapping.AddCompositeElement(nestedMapping);
        }
    }
}