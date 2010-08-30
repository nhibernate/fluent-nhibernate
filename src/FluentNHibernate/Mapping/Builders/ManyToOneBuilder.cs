using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping.Builders
{
    public class ManyToOneBuilder<TOther>
    {
        private bool nextBool = true;
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        readonly ManyToOneMapping mapping;

        public ManyToOneBuilder(ManyToOneMapping mapping, Type containingEntityType, Member member) 
        {
            this.mapping = mapping;

            InitialiseDefaults(containingEntityType, member);
        }

        void InitialiseDefaults(Type containingEntityType, Member member)
        {
            mapping.ContainingEntityType = containingEntityType;
            mapping.Member = member;
            mapping.Name = member.Name;
            mapping.SetDefaultValue(x => x.Class, new TypeReference(typeof(TOther)));
            mapping.AddDefaultColumn(CreateColumn(member.Name + "_id"));
        }

        /// <summary>
        /// Set the fetching strategy
        /// </summary>
        /// <example>
        /// Fetch.Select();
        /// </example>
        public FetchTypeExpression<ManyToOneBuilder<TOther>> Fetch
		{
            get { return new FetchTypeExpression<ManyToOneBuilder<TOther>>(this, value => mapping.Fetch = value); }
		}

        /// <summary>
        /// Set the behaviour for when this relationship is null in the database
        /// </summary>
        /// <example>
        /// NotFound.Exception();
        /// </example>
        public NotFoundExpression<ManyToOneBuilder<TOther>> NotFound
        {
            get { return new NotFoundExpression<ManyToOneBuilder<TOther>>(this, value => mapping.NotFound = value); }
        }

        /// <summary>
        /// Sets whether this relationship is unique
        /// </summary>
        /// <example>
        /// Unique();
        /// Not.Unique();
        /// </example>
        public ManyToOneBuilder<TOther> Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public ManyToOneBuilder<TOther> UniqueKey(string keyName)
        {
            columnAttributes.Set(x => x.UniqueKey, keyName);
            return this;
        }

        /// <summary>
        /// Specifies the index name
        /// </summary>
        /// <param name="indexName">Index name</param>
        public ManyToOneBuilder<TOther> Index(string indexName)
        {
            columnAttributes.Set(x => x.Index, indexName);
            return this;
        }

        /// <summary>
        /// Specifies the child class of this relationship
        /// </summary>
        /// <typeparam name="T">Child</typeparam>
        public ManyToOneBuilder<TOther> Class<T>()
        {
	        return Class(typeof(T));
        }

        /// <summary>
        /// Specifies the child class of this relationship
        /// </summary>
        /// <param name="type">Child</param>
        public ManyToOneBuilder<TOther> Class(Type type)
        {
            mapping.Class = new TypeReference(type);
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
        public ManyToOneBuilder<TOther> ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
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
        public ManyToOneBuilder<TOther> LazyLoad()
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
        public ManyToOneBuilder<TOther> LazyLoad(Laziness laziness)
        {
            mapping.Lazy = laziness.ToString();
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies this relationship should be created with a default-named
        /// foreign key.
        /// </summary>
        public ManyToOneBuilder<TOther> ForeignKey()
		{
            return ForeignKey(string.Format("FK_{0}To{1}", mapping.Member.DeclaringType.Name, mapping.Name));
		}

        /// <summary>
        /// Specifies the foreign-key constraint name
        /// </summary>
        /// <param name="foreignKeyName">Constraint name</param>
        public ManyToOneBuilder<TOther> ForeignKey(string foreignKeyName)
		{
		    mapping.ForeignKey = foreignKeyName;
			return this;
		}

        /// <summary>
        /// Specifies that this relationship is insertable
        /// </summary>
        public ManyToOneBuilder<TOther> Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies that this relationship is updatable
        /// </summary>
        public ManyToOneBuilder<TOther> Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public ManyToOneBuilder<TOther> Column(string columnName)
        {
            Columns.Clear();
            Columns.Add(columnName);
            return this;
        }

        /// <summary>
        /// Modify the columns collection
        /// </summary>
        public ColumnMappingCollection<ManyToOneBuilder<TOther>> Columns
        {
            get { return new ColumnMappingCollection<ManyToOneBuilder<TOther>>(this, mapping, columnAttributes.InnerStore); }
        }

        /// <summary>
        /// Specifies the sql formula used for this relationship
        /// </summary>
        /// <param name="formula">Formula</param>
        public ManyToOneBuilder<TOther> Formula(string formula)
        {
            mapping.Formula = formula;
            return this;
        }

        /// <summary>
        /// Specifies the cascade behaviour for this relationship
        /// </summary>
        /// <example>
        /// Cascade.All();
        /// </example>
        public CascadeExpression<ManyToOneBuilder<TOther>> Cascade
		{
			get { return new CascadeExpression<ManyToOneBuilder<TOther>>(this, value => mapping.Cascade = value); }
		}

        /// <summary>
        /// Specifies the property reference
        /// </summary>
        /// <param name="expression">Property</param>
        public ManyToOneBuilder<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
        {
            var member = expression.ToMember();

            return PropertyRef(member.Name);
        }

        /// <summary>
        /// Specifies the property reference
        /// </summary>
        /// <param name="property">Property</param>
        public ManyToOneBuilder<TOther> PropertyRef(string property)
        {
            mapping.PropertyRef = property;
            return this;
        }

        /// <summary>
        /// Sets this relationship to nullable
        /// </summary>
        /// <example>
        /// Nullable();
        /// Not.Nullable();
        /// </example>
        public ManyToOneBuilder<TOther> Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public ManyToOneBuilder<TOther> EntityName(string entityName)
        {
            mapping.EntityName = entityName;
            return this;
        }

        /// <summary>
        /// Specifies the access strategy for this relationship
        /// </summary>
        /// <example>
        /// Access.Field();
        /// </example>
        public AccessStrategyBuilder<ManyToOneBuilder<TOther>> Access
        {
            get { return new AccessStrategyBuilder<ManyToOneBuilder<TOther>>(this, value => mapping.Access = value); }
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ManyToOneBuilder<TOther> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        ColumnMapping CreateColumn(string column)
        {
            return new ColumnMapping(columnAttributes.InnerStore) { Name = column };
        }
    }
}
