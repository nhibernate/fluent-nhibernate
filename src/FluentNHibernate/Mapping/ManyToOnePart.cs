using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ManyToOnePart<TOther> : IManyToOneMappingProvider
    {
        private readonly AccessStrategyBuilder<ManyToOnePart<TOther>> access;
        private readonly FetchTypeExpression<ManyToOnePart<TOther>> fetch;
        private readonly NotFoundExpression<ManyToOnePart<TOther>> notFound;
        private readonly CascadeExpression<ManyToOnePart<TOther>> cascade;
        private readonly IList<string> columns = new List<string>();
        private bool nextBool = true;
        private readonly AttributeStore<ManyToOneMapping> attributes = new AttributeStore<ManyToOneMapping>();
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly Type entity;
        private readonly Member property;

        public ManyToOnePart(Type entity, Member property) 
        {
            this.entity = entity;
            this.property = property;
            access = new AccessStrategyBuilder<ManyToOnePart<TOther>>(this, value => attributes.Set(x => x.Access, value));
            fetch = new FetchTypeExpression<ManyToOnePart<TOther>>(this, value => attributes.Set(x => x.Fetch, value));
            cascade = new CascadeExpression<ManyToOnePart<TOther>>(this, value => attributes.Set(x => x.Cascade, value));
            notFound = new NotFoundExpression<ManyToOnePart<TOther>>(this, value => attributes.Set(x => x.NotFound, value));
        }

        ManyToOneMapping IManyToOneMappingProvider.GetManyToOneMapping()
        {
            var mapping = new ManyToOneMapping(attributes.CloneInner());

            mapping.ContainingEntityType = entity;
            mapping.Member = property;

            if (!mapping.IsSpecified("Name"))
                mapping.Name = property.Name;

            if (!mapping.IsSpecified("Class"))
                mapping.SetDefaultValue(x => x.Class, new TypeReference(typeof(TOther)));

            if (columns.Count == 0)
                mapping.AddDefaultColumn(CreateColumn(property.Name + "_id"));

            foreach (var column in columns)
            {
                var columnMapping = CreateColumn(column);

                mapping.AddColumn(columnMapping);
            }

            return mapping;
        }

        private ColumnMapping CreateColumn(string column)
        {
            return new ColumnMapping(columnAttributes.CloneInner()) { Name = column };
        }

        public FetchTypeExpression<ManyToOnePart<TOther>> Fetch
		{
			get { return fetch; }
		}

        public NotFoundExpression<ManyToOnePart<TOther>> NotFound
        {
            get { return notFound; }
        }

        public ManyToOnePart<TOther> Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public ManyToOnePart<TOther> UniqueKey(string keyName)
        {
            columnAttributes.Set(x => x.UniqueKey, keyName);
            return this;
        }

        public ManyToOnePart<TOther> Index(string indexName)
        {
            columnAttributes.Set(x => x.Index, indexName);
            return this;
        }

        public ManyToOnePart<TOther> Class<T>()
        {
	        return Class(typeof(T));
        }

        public ManyToOnePart<TOther> Class(Type type)
        {
            attributes.Set(x => x.Class, new TypeReference(type));
            return this;
        }

        public ManyToOnePart<TOther> ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            attributes.Set(x => x.Update, !nextBool);
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
            attributes.Set(x => x.Lazy, laziness.ToString());
            nextBool = true;
            return this;
        }
		
		public ManyToOnePart<TOther> ForeignKey()
		{
			return ForeignKey(string.Format("FK_{0}To{1}", property.DeclaringType.Name, property.Name));
		}
		
		public ManyToOnePart<TOther> ForeignKey(string foreignKeyName)
		{
		    attributes.Set(x => x.ForeignKey, foreignKeyName);
			return this;
		}

        public ManyToOnePart<TOther> Insert()
        {
            attributes.Set(x => x.Insert, nextBool);
            nextBool = true;
            return this;
        }

        public ManyToOnePart<TOther> Update()
        {
            attributes.Set(x => x.Update, nextBool);
            nextBool = true;
            return this;
        }

        public ManyToOnePart<TOther> Columns(params string[] columns)
        {
            foreach (var column in columns)
            {
                this.columns.Add(column);
            }

            return this;
        }

        public ManyToOnePart<TOther> Columns(params Expression<Func<TOther, object>>[] columns)
        {
            foreach (var expression in columns)
            {
                var member = expression.ToMember();

                Columns(member.Name);
            }

            return this;
        }

        public ManyToOnePart<TOther> Formula(string formula)
        {
            attributes.Set(x => x.Formula, formula);
            return this;
        }

        public CascadeExpression<ManyToOnePart<TOther>> Cascade
		{
			get { return cascade; }
		}

        public ManyToOnePart<TOther> Column(string name)
        {
            columns.Clear();
            columns.Add(name);

            return this;
        }

        public ManyToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> expression)
        {
            var member = expression.ToMember();

            return PropertyRef(member.Name);
        }

        public ManyToOnePart<TOther> PropertyRef(string property)
        {
            attributes.Set(x => x.PropertyRef, property);
            return this;
        }

        public ManyToOnePart<TOther> Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public ManyToOnePart<TOther> EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
            return this;
        }

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
    }
}
