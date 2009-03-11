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
        Type ParentType { get; }
        Type ChildType { get; }
        void WithChildKeyColumn(string name);
        void WithParentKeyColumn(string name);
    }

	public class ManyToManyPart<PARENT, CHILD> : ToManyBase<ManyToManyPart<PARENT, CHILD>, PARENT, CHILD>, IManyToManyPart
    {
        public string ChildKeyColumn { get; private set; }
        public string ParentKeyColumn { get; private set; }
        private readonly Cache<string, string> _parentKeyProperties = new Cache<string, string>();
        private readonly Cache<string, string> _manyToManyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>> access;

	    public ManyToManyPart(PropertyInfo property)
            : this(property, property.PropertyType)
	    {}

	    public ManyToManyPart(MethodInfo method)
	        : this(method, method.ReturnType)
        {}

        protected ManyToManyPart(MemberInfo member, Type collectionType)
            : base(member, collectionType)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>>(this);
            _properties.Store("name", member.Name);
        }

	    public ManyToManyPart<PARENT, CHILD> WithChildKeyColumn(string childKeyColumn)
		{
			ChildKeyColumn = childKeyColumn;
			return this;
		}
		
		public ManyToManyPart<PARENT, CHILD> WithParentKeyColumn(string parentKeyColumn)
		{
		    ParentKeyColumn = parentKeyColumn;
			return this;
		}

        public ManyToManyPart<PARENT, CHILD> WithForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName) {
            _parentKeyProperties.Store("foreign-key", parentForeignKeyName);
            _manyToManyProperties.Store("foreign-key", childForeignKeyName);
            return this;
        }
		
		public FetchTypeExpression<ManyToManyPart<PARENT, CHILD>> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToManyPart<PARENT, CHILD>>(this, _manyToManyProperties);
			}
		}

		public override void Write(XmlElement classElement, IMappingVisitor visitor)
        {
		    XmlElement set = classElement.AddElement(_collectionType).WithProperties(_properties);

            if (!string.IsNullOrEmpty(TableName))
                set.WithAtt("table", TableName);

			XmlElement key = set.AddElement("key");
			key.WithAtt("column", ParentKeyColumn);
		    key.WithProperties(_parentKeyProperties);

			XmlElement manyToManyElement = set.AddElement("many-to-many");
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

        public override int Level
        {
            get { return 3; }
        }

	    public override PartPosition Position
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

	    public Type ParentType
	    {
            get { return typeof(PARENT); }
	    }

        public Type ChildType
        {
            get { return typeof(CHILD); }
        }
    }
}
