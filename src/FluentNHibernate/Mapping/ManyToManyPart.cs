using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IManyToManyPart : ICollectionRelationship
    {
        void Inverse();
        void WithTableName(string tableName);
        string ChildKeyColumn { get; }
        string ParentKeyColumn { get; }
        Type ChildType { get; }
        void WithChildKeyColumn(string name);
        void WithParentKeyColumn(string name);
        INotFoundExpression NotFound { get; }
        CollectionCascadeExpression<IManyToManyPart> Cascade { get; }
    }

	public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild>, IManyToManyPart
    {
        public string ChildKeyColumn { get; private set; }
        public string ParentKeyColumn { get; private set; }
        private readonly Cache<string, string> parentKeyProperties = new Cache<string, string>();
        private readonly Cache<string, string> manyToManyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ManyToManyPart<TChild>> access;

	    public ManyToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
	    {}

	    public ManyToManyPart(Type entity, MethodInfo method)
	        : this(entity, method, method.ReturnType)
        {}

        protected ManyToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<TChild>>(this);
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
			get
			{
				return new FetchTypeExpression<ManyToManyPart<TChild>>(this, manyToManyProperties);
			}
		}

		public override void Write(XmlElement classElement, IMappingVisitor visitor)
        {
		    XmlElement collectionElement = classElement.AddElement(collectionType).WithProperties(properties);

            if (!string.IsNullOrEmpty(TableName))
                collectionElement.WithAtt("table", TableName);

            if (batchSize > 0)
                collectionElement.WithAtt("batch-size", batchSize.ToString());

		    Cache.Write(collectionElement, visitor);

            XmlElement key = collectionElement.AddElement("key");
			key.WithAtt("column", ParentKeyColumn);
		    key.WithProperties(parentKeyProperties);

            if (indexMapping != null)
                WriteIndexElement(collectionElement);

			XmlElement manyToManyElement = collectionElement.AddElement("many-to-many");
			manyToManyElement.WithAtt("column", ChildKeyColumn);
			manyToManyElement.WithAtt("class", typeof(TChild).AssemblyQualifiedName);
			manyToManyElement.WithProperties(manyToManyProperties);
        }

    	/// <summary>
        /// Set an attribute on the xml element produced by this many-to-many mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public override void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
        }

        public override void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public override int LevelWithinPosition
        {
            get { return 1; }
        }

	    public override PartPosition PositionOnDocument
	    {
            get { return PartPosition.Anywhere; }
	    }

        void IManyToManyPart.Inverse()
        {
            Inverse();
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
            get
            {
                return new NotFoundExpression<ManyToManyPart<TChild>>(this, manyToManyProperties);
            }
        }

        INotFoundExpression IManyToManyPart.NotFound
        {
            get { return NotFound; }
        }

        CollectionCascadeExpression<IManyToManyPart> IManyToManyPart.Cascade
        {
            get { return new CollectionCascadeExpression<IManyToManyPart>(this); }
        }
    }
}
