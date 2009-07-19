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
	public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild, ManyToManyMapping>
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

        public Type ChildType
        {
            get { return typeof(TChild); }
        }

        public NotFoundExpression<ManyToManyPart<TChild>> NotFound
        {
            get { return notFound; }
        }

	    protected override ICollectionRelationshipMapping GetRelationship()
	    {
	        return new ManyToManyMapping(relationshipAttributes.CloneInner())
	        {
	            ContainingEntityType = entity
	        };
	    }
    }
}
