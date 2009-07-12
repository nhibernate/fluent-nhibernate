using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public interface IManyToManyPart : ICollectionRelationship
    {
        new IManyToManyPart Not { get; }
        void WithTableName(string tableName);
        Type ChildType { get; }
        void WithChildKeyColumn(string name);
        void WithParentKeyColumn(string name);
        void WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName);
        INotFoundExpression NotFound { get; }
        new CollectionCascadeExpression<IManyToManyPart> Cascade { get; }
        OuterJoinBuilder<IManyToManyPart> OuterJoin { get; }
        FetchTypeExpression<IManyToManyPart> Fetch { get; }
        OptimisticLockBuilder<IManyToManyPart> OptimisticLock { get; }
        IManyToManyPart SchemaIs(string schema);
        IManyToManyPart Check(string checkSql);
        IManyToManyPart Generic();
        IManyToManyPart Persister<T>() where T : IEntityPersister;
    }

	public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild, ManyToManyMapping>, IManyToManyPart
    {
	    private readonly Type entity;
	    private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
	    private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
	    private IndexManyToManyPart manyToManyIndex;
	    private readonly IList<string> childColumns = new List<string>();
	    private readonly IList<string> parentColumns = new List<string>();

	    public ManyToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
	    {}

	    public ManyToManyPart(Type entity, MethodInfo method)
	        : this(entity, method, method.ReturnType)
	    {}

	    protected ManyToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
	        this.entity = entity;

	        fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Fetch, value));
            notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, value => relationshipAttributes.Set(x => x.NotFound, value));
        }

        public override ICollectionMapping GetCollectionMapping()
        {
            var collection = base.GetCollectionMapping();

            // key columns
            if (parentColumns.Count == 0)
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = entity.Name + "Id" });

            foreach (var column in parentColumns)
                collection.Key.AddColumn(new ColumnMapping { Name = column });

            // child columns
            if (childColumns.Count == 0)
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = typeof(TChild).Name + "Id" });

            foreach (var column in childColumns)
                ((ManyToManyMapping)collection.Relationship).AddColumn(new ColumnMapping { Name = column });

            // HACK: shouldn't have to do this!
            if (manyToManyIndex != null && collection is MapMapping)
                ((MapMapping)collection).Index = manyToManyIndex.GetIndexMapping();

            return collection;
        }

	    public ManyToManyPart<TChild> WithChildKeyColumn(string childKeyColumn)
		{
	        childColumns.Clear(); // support only one currently
	        childColumns.Add(childKeyColumn);
			return this;
		}
		
		public ManyToManyPart<TChild> WithParentKeyColumn(string parentKeyColumn)
		{
            parentColumns.Clear(); // support only one currently
            parentColumns.Add(parentKeyColumn);
			return this;
		}

        public ManyToManyPart<TChild> WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            keyAttributes.Set(x => x.ForeignKey, parentForeignKeyName);
            relationshipAttributes.Set(x => x.ForeignKey, childForeignKeyName);
            return this;
        }
		
		public FetchTypeExpression<ManyToManyPart<TChild>> FetchType
		{
			get { return fetch; }
		}

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsTernaryAssociation(indexSelector, null);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexManyToManyPart> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsTernaryAssociation<TIndex>(indexProperty.Name, customIndexMapping);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(string indexColumn)
        {
            return AsTernaryAssociation<TIndex>(indexColumn, null);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(string indexColumn, Action<IndexManyToManyPart> customIndexMapping)
        {
            manyToManyIndex = new IndexManyToManyPart();
            manyToManyIndex.WithColumn(indexColumn);
            manyToManyIndex.WithType<TIndex>();

            if (customIndexMapping != null)
                customIndexMapping(manyToManyIndex);

            return this;
        }

	    void IManyToManyPart.WithTableName(string tableName)
        {
            WithTableName(tableName);
        }

        void IManyToManyPart.WithChildKeyColumn(string name)
        {
            WithChildKeyColumn(name);
        }

        void IManyToManyPart.WithParentKeyColumn(string name)
        {
            WithParentKeyColumn(name);
        }

        void IManyToManyPart.WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            WithForeignKeyConstraintNames(parentForeignKeyName, childForeignKeyName);
        }

        IManyToManyPart IManyToManyPart.SchemaIs(string schema)
        {
            return SchemaIs(schema);
        }

        IManyToManyPart IManyToManyPart.Check(string checkSql)
        {
            return Check(checkSql);
        }

        FetchTypeExpression<IManyToManyPart> IManyToManyPart.Fetch
        {
            get { return new FetchTypeExpression<IManyToManyPart>(this, value => collectionAttributes.Set(x => x.Fetch, value)); }
        }

        OuterJoinBuilder<IManyToManyPart> IManyToManyPart.OuterJoin
        {
            get { return new OuterJoinBuilder<IManyToManyPart>(this, value => collectionAttributes.Set(x => x.OuterJoin, value)); }
        }

        public Type ChildType
        {
            get { return typeof(TChild); }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        public NotFoundExpression<ManyToManyPart<TChild>> NotFound
        {
            get { return notFound; }
        }

        INotFoundExpression IManyToManyPart.NotFound
        {
            get { return NotFound; }
        }

        IManyToManyPart IManyToManyPart.Not
        {
            get { return Not; }
        }

        IManyToManyPart IManyToManyPart.Generic()
        {
            return Generic();
        }

        IManyToManyPart IManyToManyPart.Persister<T>()
        {
            return Persister<T>();
        }

        OptimisticLockBuilder<IManyToManyPart> IManyToManyPart.OptimisticLock
        {
            get { return new OptimisticLockBuilder<IManyToManyPart>(this, value => collectionAttributes.Set(x => x.OptimisticLock, value)); }
        }

	    protected override ICollectionRelationshipMapping GetRelationship()
	    {
            return new ManyToManyMapping(relationshipAttributes.CloneInner());
	    }

	    CollectionCascadeExpression<IManyToManyPart> IManyToManyPart.Cascade
        {
            get { return new CollectionCascadeExpression<IManyToManyPart>(this, value => collectionAttributes.Set(x => x.Cascade, value)); }
        }
    }
}
