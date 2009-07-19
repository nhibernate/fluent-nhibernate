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
        void Table(string tableName);
        Type ChildType { get; }
        void ChildKeyColumn(string name);
        void ParentKeyColumn(string name);
        void ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName);
        INotFoundExpression NotFound { get; }
        new CollectionCascadeExpression<IManyToManyPart> Cascade { get; }
        OuterJoinBuilder<IManyToManyPart> OuterJoin { get; }
        FetchTypeExpression<IManyToManyPart> Fetch { get; }
        OptimisticLockBuilder<IManyToManyPart> OptimisticLock { get; }
        IManyToManyPart Schema(string schema);
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
                collection.Key.AddDefaultColumn(new ColumnMapping { Name = entity.Name + "_id" });

            foreach (var column in parentColumns)
                collection.Key.AddColumn(new ColumnMapping { Name = column });

            // child columns
            if (childColumns.Count == 0)
                ((ManyToManyMapping)collection.Relationship).AddDefaultColumn(new ColumnMapping { Name = typeof(TChild).Name + "_id" });

            foreach (var column in childColumns)
                ((ManyToManyMapping)collection.Relationship).AddColumn(new ColumnMapping { Name = column });

            // HACK: shouldn't have to do this!
            if (manyToManyIndex != null && collection is MapMapping)
                ((MapMapping)collection).Index = manyToManyIndex.GetIndexMapping();

            return collection;
        }

	    public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
		{
	        childColumns.Clear(); // support only one currently
	        childColumns.Add(childKeyColumn);
			return this;
		}
		
		public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
		{
            parentColumns.Clear(); // support only one currently
            parentColumns.Add(parentKeyColumn);
			return this;
		}

        public ManyToManyPart<TChild> ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
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
            manyToManyIndex.Column(indexColumn);
            manyToManyIndex.Type<TIndex>();

            if (customIndexMapping != null)
                customIndexMapping(manyToManyIndex);

            return this;
        }

	    void IManyToManyPart.Table(string tableName)
        {
            Table(tableName);
        }

        void IManyToManyPart.ChildKeyColumn(string name)
        {
            ChildKeyColumn(name);
        }

        void IManyToManyPart.ParentKeyColumn(string name)
        {
            ParentKeyColumn(name);
        }

        void IManyToManyPart.ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
        {
            ForeignKeyConstraintNames(parentForeignKeyName, childForeignKeyName);
        }

        IManyToManyPart IManyToManyPart.Schema(string schema)
        {
            return Schema(schema);
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
	        return new ManyToManyMapping(relationshipAttributes.CloneInner())
	        {
	            ContainingEntityType = entity
	        };
	    }

	    CollectionCascadeExpression<IManyToManyPart> IManyToManyPart.Cascade
        {
            get { return new CollectionCascadeExpression<IManyToManyPart>(this, value => collectionAttributes.Set(x => x.Cascade, value)); }
        }
    }
}
