using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ManyToOnePart<TOther> : IManyToOneMappingProvider
    {
        readonly AccessStrategyBuilder<ManyToOnePart<TOther>> access;
        readonly FetchTypeExpression<ManyToOnePart<TOther>> fetch;
        readonly NotFoundExpression<ManyToOnePart<TOther>> notFound;
        readonly CascadeExpression<ManyToOnePart<TOther>> cascade;
        readonly IList<string> columns = new List<string>();
        bool nextBool = true;
        readonly AttributeStore attributes = new AttributeStore();
        readonly AttributeStore columnAttributes = new AttributeStore();
        readonly Type entity;
        readonly Member member;

        public ManyToOnePart(Type entity, Member member) 
        {
            this.entity = entity;
            this.member = member;
            access = new AccessStrategyBuilder<ManyToOnePart<TOther>>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            fetch = new FetchTypeExpression<ManyToOnePart<TOther>>(this, value => attributes.Set("Fetch", Layer.UserSupplied, value));
            cascade = new CascadeExpression<ManyToOnePart<TOther>>(this, value => attributes.Set("Cascade", Layer.UserSupplied, value));
            notFound = new NotFoundExpression<ManyToOnePart<TOther>>(this, value => attributes.Set("NotFound", Layer.UserSupplied, value));

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
        /// Set the fetching strategy
        /// </summary>
        /// <example>
        /// Fetch.Select();
        /// </example>
        public FetchTypeExpression<ManyToOnePart<TOther>> Fetch
		{
			get { return fetch; }
		}

        /// <summary>
        /// Set the behaviour for when this relationship is null in the database
        /// </summary>
        /// <example>
        /// NotFound.Exception();
        /// </example>
        public NotFoundExpression<ManyToOnePart<TOther>> NotFound
        {
            get { return notFound; }
        }

        /// <summary>
        /// Sets whether this relationship is unique
        /// </summary>
        /// <example>
        /// Unique();
        /// Not.Unique();
        /// </example>
        public ManyToOnePart<TOther> Unique()
        {
            columnAttributes.Set("Unique", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public ManyToOnePart<TOther> UniqueKey(string keyName)
        {
            columnAttributes.Set("UniqueKey", Layer.UserSupplied, keyName);
            return this;
        }

        /// <summary>
        /// Specifies the index name
        /// </summary>
        /// <param name="indexName">Index name</param>
        public ManyToOnePart<TOther> Index(string indexName)
        {
            columnAttributes.Set("Index", Layer.UserSupplied, indexName);
            return this;
        }

        /// <summary>
        /// Specifies the child class of this relationship
        /// </summary>
        /// <typeparam name="T">Child</typeparam>
        public ManyToOnePart<TOther> Class<T>()
        {
	        return Class(typeof(T));
        }

        /// <summary>
        /// Specifies the child class of this relationship
        /// </summary>
        /// <param name="type">Child</param>
        public ManyToOnePart<TOther> Class(Type type)
        {
            attributes.Set("Class", Layer.UserSupplied, new TypeReference(type));
            return this;
        }

        /// <summary>
        /// Sets this relationship to read-only
        /// </summary>
        /// <remarks>
        /// This is the same as calling both Not.Insert() and Not.Update()
        /// </remarks>
        /// <example>
        /// ReadOnly();
        /// Not.ReadOnly();
        /// </example>
        public ManyToOnePart<TOther> ReadOnly()
        {
            attributes.Set("Insert", Layer.UserSupplied, !nextBool);
            attributes.Set("Update", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
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
        public ManyToOnePart<TOther> LazyLoad()
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
        public ManyToOnePart<TOther> LazyLoad(Laziness laziness)
        {
            attributes.Set("Lazy", Layer.UserSupplied, laziness.ToString());
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies this relationship should be created with a default-named
        /// foreign key.
        /// </summary>
        public ManyToOnePart<TOther> ForeignKey()
		{
			return ForeignKey(string.Format("FK_{0}To{1}", member.DeclaringType.Name, member.Name));
		}

        /// <summary>
        /// Specifies the foreign-key constraint name
        /// </summary>
        /// <param name="foreignKeyName">Constraint name</param>
        public ManyToOnePart<TOther> ForeignKey(string foreignKeyName)
		{
            attributes.Set("ForeignKey", Layer.UserSupplied, foreignKeyName);
			return this;
		}

        /// <summary>
        /// Specifies that this relationship is insertable
        /// </summary>
        public ManyToOnePart<TOther> Insert()
        {
            attributes.Set("Insert", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies that this relationship is updatable
        /// </summary>
        public ManyToOnePart<TOther> Update()
        {
            attributes.Set("Update", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Sets the single column used in this relationship. Use <see cref="Columns(string[])"/>
        /// if you need to specify more than one column.
        /// </summary>
        /// <param name="name">Column name</param>
        public ManyToOnePart<TOther> Column(string name)
        {
            columns.Clear();
            columns.Add(name);

            return this;
        }

        /// <summary>
        /// Specifies the columns used in this relationship
        /// </summary>
        /// <param name="newColumns">Columns</param>
        public ManyToOnePart<TOther> Columns(params string[] newColumns)
        {
            foreach (var column in newColumns)
            {
                columns.Add(column);
            }

            return this;
        }

        /// <summary>
        /// Specifies the columns used in this relationship
        /// </summary>
        /// <param name="newColumns">Columns</param>
        public ManyToOnePart<TOther> Columns(params Expression<Func<TOther, object>>[] newColumns)
        {
            foreach (var expression in newColumns)
            {
                Columns(expression.ToMember().Name);
            }

            return this;
        }

        /// <summary>
        /// Specifies the sql formula used for this relationship
        /// </summary>
        /// <param name="formula">Formula</param>
        public ManyToOnePart<TOther> Formula(string formula)
        {
            attributes.Set("Formula", Layer.UserSupplied, formula);
            return this;
        }

        /// <summary>
        /// Specifies the cascade behaviour for this relationship
        /// </summary>
        /// <example>
        /// Cascade.All();
        /// </example>
        public CascadeExpression<ManyToOnePart<TOther>> Cascade
		{
			get { return cascade; }
		}

        /// <summary>
        /// Specifies the property reference
        /// </summary>
        /// <param name="expression">Property</param>
        public ManyToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
        {
            return PropertyRef(expression.ToMember().Name);
        }

        /// <summary>
        /// Specifies the property reference
        /// </summary>
        /// <param name="property">Property</param>
        public ManyToOnePart<TOther> PropertyRef(string property)
        {
            attributes.Set("PropertyRef", Layer.UserSupplied, property);
            return this;
        }

        /// <summary>
        /// Sets this relationship to nullable
        /// </summary>
        /// <example>
        /// Nullable();
        /// Not.Nullable();
        /// </example>
        public ManyToOnePart<TOther> Nullable()
        {
            columnAttributes.Set("NotNull", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public ManyToOnePart<TOther> EntityName(string entityName)
        {
            attributes.Set("EntityName", Layer.UserSupplied, entityName);
            return this;
        }

        /// <summary>
        /// Specifies the access strategy for this relationship
        /// </summary>
        /// <example>
        /// Access.Field();
        /// </example>
        public AccessStrategyBuilder<ManyToOnePart<TOther>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ManyToOnePart<TOther> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        ManyToOneMapping IManyToOneMappingProvider.GetManyToOneMapping()
        {
            var mapping = new ManyToOneMapping(attributes.Clone())
            {
                ContainingEntityType = entity,
                Member = member
            };

            mapping.Set(x => x.Name, Layer.Defaults, member.Name);
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(TOther)));

            if (columns.Count == 0 && !mapping.IsSpecified("Formula"))
                mapping.AddColumn(Layer.Defaults, CreateColumn(member.Name + "_id"));

            foreach (var column in columns)
            {
                var columnMapping = CreateColumn(column);

                mapping.AddColumn(Layer.UserSupplied, columnMapping);
            }

            return mapping;
        }

        ColumnMapping CreateColumn(string column)
        {
            var columnMapping = new ColumnMapping(columnAttributes.Clone());
            columnMapping.Set(x => x.Name, Layer.Defaults, column);
            return columnMapping;
        }

        public ManyToOnePart<TOther> OptimisticLock()
        {
            attributes.Set("OptimisticLock", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }
    }
}
