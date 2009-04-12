using System;
using System.Reflection;
using System.Xml;

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

	public class ManyToManyPart<CHILD> : ToManyBase<ManyToManyPart<CHILD>, CHILD>, IManyToManyPart
    {
        public string ChildKeyColumn { get; private set; }
        public string ParentKeyColumn { get; private set; }
        private readonly Cache<string, string> _parentKeyProperties = new Cache<string, string>();
        private readonly Cache<string, string> _manyToManyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ManyToManyPart<CHILD>> access;

	    public ManyToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
	    {}

	    public ManyToManyPart(Type entity, MethodInfo method)
	        : this(entity, method, method.ReturnType)
        {}

        protected ManyToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<CHILD>>(this);
            _properties.Store("name", member.Name);
        }

	    public ManyToManyPart<CHILD> WithChildKeyColumn(string childKeyColumn)
		{
			ChildKeyColumn = childKeyColumn;
			return this;
		}
		
		public ManyToManyPart<CHILD> WithParentKeyColumn(string parentKeyColumn)
		{
		    ParentKeyColumn = parentKeyColumn;
			return this;
		}

        public ManyToManyPart<CHILD> WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName) {
            _parentKeyProperties.Store("foreign-key", parentForeignKeyName);
            _manyToManyProperties.Store("foreign-key", childForeignKeyName);
            return this;
        }
		
		public FetchTypeExpression<ManyToManyPart<CHILD>> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToManyPart<CHILD>>(this, _manyToManyProperties);
			}
		}

		public override void Write(XmlElement classElement, IMappingVisitor visitor)
        {
		    XmlElement collectionElement = classElement.AddElement(_collectionType).WithProperties(_properties);

            if (!string.IsNullOrEmpty(TableName))
                collectionElement.WithAtt("table", TableName);

            if (batchSize > 0)
                collectionElement.WithAtt("batch-size", batchSize.ToString());

		    Cache.Write(collectionElement, visitor);

            if (_indexMapping != null)
                WriteIndexElement(collectionElement);

			XmlElement key = collectionElement.AddElement("key");
			key.WithAtt("column", ParentKeyColumn);
		    key.WithProperties(_parentKeyProperties);

			XmlElement manyToManyElement = collectionElement.AddElement("many-to-many");
			manyToManyElement.WithAtt("column", ChildKeyColumn);
			manyToManyElement.WithAtt("class", typeof(CHILD).AssemblyQualifiedName);
			manyToManyElement.WithProperties(_manyToManyProperties);
        }

    	/// <summary>
        /// Set an attribute on the xml element produced by this many-to-many mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public override void SetAttribute(string name, string value)
        {
            _properties.Store(name, value);
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
            this.Inverse();
        }

        void IManyToManyPart.WithTableName(string tableName)
        {
            this.WithTableName(tableName);
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
            get { return typeof(CHILD); }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        public NotFoundExpression<ManyToManyPart<CHILD>> NotFound
        {
            get
            {
                return new NotFoundExpression<ManyToManyPart<CHILD>>(this, _manyToManyProperties);
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
