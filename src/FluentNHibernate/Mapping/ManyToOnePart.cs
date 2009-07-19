using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IManyToOnePart : IRelationship
    {
        CascadeExpression<IManyToOnePart> Cascade { get; }
        PropertyInfo Property { get; }
        IManyToOnePart Column(string columnName);
        INotFoundExpression NotFound { get; }
        FetchTypeExpression<IManyToOnePart> Fetch { get; }
        IManyToOnePart Not { get; }
        OuterJoinBuilder<IManyToOnePart> OuterJoin { get; }
        ManyToOneMapping GetManyToOneMapping();
        IManyToOnePart ForeignKey(string foreignKeyName);
        IManyToOnePart Insert();
        IManyToOnePart Update();
        IManyToOnePart ReadOnly();
        IManyToOnePart LazyLoad();
        IManyToOnePart PropertyRef(string property);
        IManyToOnePart Nullable();
        IManyToOnePart Unique();
        IManyToOnePart UniqueKey(string uniqueConstraintName);
        IManyToOnePart Index(string indexName);
        IManyToOnePart Class(Type type);
        IManyToOnePart Class<T>();
    }

    public class ManyToOnePart<TOther> : IManyToOnePart, IAccessStrategy<ManyToOnePart<TOther>>
    {
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private readonly AccessStrategyBuilder<ManyToOnePart<TOther>> access;
        private readonly FetchTypeExpression<IManyToOnePart> fetch;
        private readonly NotFoundExpression<ManyToOnePart<TOther>> notFound;
        private readonly CascadeExpression<IManyToOnePart> cascade;
        private readonly IList<string> columns = new List<string>();
        private bool nextBool = true;
        private readonly ManyToOneMapping mapping;
        private readonly OuterJoinBuilder<IManyToOnePart> outerJoin;
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();

        public ManyToOnePart(Type entity, PropertyInfo property) 
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<ManyToOnePart<TOther>>(this, value => mapping.Access = value);
            fetch = new FetchTypeExpression<IManyToOnePart>(this, value => mapping.Fetch = value);
            cascade = new CascadeExpression<IManyToOnePart>(this, value => mapping.Cascade = value);
            notFound = new NotFoundExpression<ManyToOnePart<TOther>>(this, value => mapping.NotFound = value);
            outerJoin = new OuterJoinBuilder<IManyToOnePart>(this, value => mapping.OuterJoin = value);

            Property = property;

            mapping = new ManyToOneMapping { ContainingEntityType = entity };
        }

        public OuterJoinBuilder<IManyToOnePart> OuterJoin
        {
            get { return outerJoin; }
        }

        ManyToOneMapping IManyToOnePart.GetManyToOneMapping()
        {
            mapping.PropertyInfo = Property;

            if (!mapping.IsSpecified(x => x.Name))
                mapping.Name = Property.Name;

            if (!mapping.IsSpecified(x => x.Class))
                mapping.Class = new TypeReference(Property.PropertyType);

            if (columns.Count == 0)
                mapping.AddDefaultColumn(CreateColumn(Property.Name + "_id"));

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

        public FetchTypeExpression<IManyToOnePart> Fetch
		{
			get { return fetch; }
		}
        
        IManyToOnePart IManyToOnePart.Not
        {
            get { return Not; }
        }

        public NotFoundExpression<ManyToOnePart<TOther>> NotFound
        {
            get { return notFound; }
        }

        INotFoundExpression IManyToOnePart.NotFound
        {
            get { return NotFound; }
        }

        public ManyToOnePart<TOther> Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        IManyToOnePart IManyToOnePart.Unique()
        {
            return Unique();
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

        public IManyToOnePart Index(string indexName)
        {
            columnAttributes.Set(x => x.Index, indexName);
            return this;
        }

        public IManyToOnePart Class<T>()
        {
	        return Class(typeof(T));
        }

        public IManyToOnePart Class(Type type)
        {
            mapping.Class = new TypeReference(type);
            return this;
        }

        IManyToOnePart IManyToOnePart.UniqueKey(string uniqueConstraintName)
        {
            return UniqueKey(uniqueConstraintName);
        }

        public IManyToOnePart ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public ManyToOnePart<TOther> LazyLoad()
        {
            mapping.Lazy = nextBool ? Laziness.Proxy : Laziness.False;
            nextBool = true;
            return this;
        }

        IManyToOnePart IManyToOnePart.LazyLoad()
        {
            return LazyLoad();
        }
		
		public ManyToOnePart<TOther> ForeignKey()
		{
			return ForeignKey(string.Format("FK_{0}To{1}", Property.DeclaringType.Name, Property.Name));
		}
		
		public ManyToOnePart<TOther> ForeignKey(string foreignKeyName)
		{
		    mapping.ForeignKey = foreignKeyName;
			return this;
		}

        public IManyToOnePart Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        public IManyToOnePart Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
        }

        IManyToOnePart IManyToOnePart.ForeignKey(string foreignKeyName)
        {
            return ForeignKey(foreignKeyName);
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
                var property = ReflectionHelper.GetProperty(expression);

                Columns(property.Name);
            }

            return this;
        }
		
		public CascadeExpression<IManyToOnePart> Cascade
		{
			get { return cascade; }
		}

        IManyToOnePart IManyToOnePart.Column(string name)
        {
            return Column(name);
        }

        public ManyToOnePart<TOther> Column(string name)
        {
            columns.Clear();
            columns.Add(name);

            return this;
        }

        public ManyToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> propertyRef)
        {
            var property = ReflectionHelper.GetProperty(propertyRef);

            return PropertyRef(property.Name);
        }

        public ManyToOnePart<TOther> PropertyRef(string property)
        {
            mapping.PropertyRef = property;
            return this;
        }

        IManyToOnePart IManyToOnePart.PropertyRef(string property)
        {
            return PropertyRef(property);
        }

        public ManyToOnePart<TOther> Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        IManyToOnePart IManyToOnePart.Nullable()
        {
            return Nullable();
        }

        public AccessStrategyBuilder<ManyToOnePart<TOther>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public ManyToOnePart<TOther> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        CascadeExpression<IManyToOnePart> IManyToOnePart.Cascade
        {
            get { return cascade; }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }
    }
}
