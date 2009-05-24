using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IManyToManyPart : ICollectionRelationship
    {
        void WithTableName(string tableName);
        string ChildKeyColumn { get; }
        string ParentKeyColumn { get; }
        Type ChildType { get; }
        void WithChildKeyColumn(string name);
        void WithParentKeyColumn(string name);
        INotFoundExpression NotFound { get; }
        CollectionCascadeExpression<IManyToManyPart> Cascade { get; }
    }

	public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild, ManyToManyMapping>, IManyToManyPart
    {
        public string ChildKeyColumn { get; private set; }
        public string ParentKeyColumn { get; private set; }
        private readonly Cache<string, string> parentKeyProperties = new Cache<string, string>();
        private readonly Cache<string, string> manyToManyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ManyToManyPart<TChild>> access;
	    private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
	    private IndexMapping manyToManyIndex;
	    private NotFoundExpression<ManyToManyPart<TChild>> notFound;

	    public ManyToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
	    {}

	    public ManyToManyPart(Type entity, MethodInfo method)
	        : this(entity, method, method.ReturnType)
        {}

        protected ManyToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Access, value));
            fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, value => collectionAttributes.Set(x => x.Fetch, value));
            notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, value => relationshipAttributes.Set(x => x.NotFound, value));
            properties.Store("name", member.Name);
        }

	    public ManyToManyPart<TChild> WithChildKeyColumn(string childKeyColumn)
		{
			ChildKeyColumn = childKeyColumn;
			return this;
		}
		
		public ManyToManyPart<TChild> WithParentKeyColumn(string parentKeyColumn)
		{
		    ParentKeyColumn = parentKeyColumn;
			return this;
		}

        public ManyToManyPart<TChild> WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName) {
            parentKeyProperties.Store("foreign-key", parentForeignKeyName);
            manyToManyProperties.Store("foreign-key", childForeignKeyName);
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

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsTernaryAssociation<TIndex>(indexProperty.Name, customIndexMapping);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(string indexColumn)
        {
            return AsTernaryAssociation<TIndex>(indexColumn, null);
        }

        public ManyToManyPart<TChild> AsTernaryAssociation<TIndex>(string indexColumn, Action<IndexMapping> customIndexMapping)
        {
            manyToManyIndex = new IndexMapping();
            manyToManyIndex.WithColumn(indexColumn);
            manyToManyIndex.WithType<TIndex>();

            if (customIndexMapping != null)
                customIndexMapping(manyToManyIndex);

            return this;
        }

        protected void WriteIndexManyToManyElement(XmlElement collectionElement)
        {
            var indexElement = collectionElement.AddElement("index-many-to-many");
            manyToManyIndex.WriteAttributesToIndexElement(indexElement);
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

	    protected override ICollectionRelationshipMapping GetRelationship()
	    {
            var relationship = new ManyToManyMapping();

            relationshipAttributes.CopyTo(relationship.Attributes);

            return relationship;
	    }

	    CollectionCascadeExpression<IManyToManyPart> IManyToManyPart.Cascade
        {
            get { return Cascade; }
        }
    }
}
