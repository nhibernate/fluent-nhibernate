using System;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
	public class ManyToManyPart<PARENT, CHILD> : ToManyBase<ManyToManyPart<PARENT, CHILD>, PARENT, CHILD>
    {
    	private string _childKeyColumn;
        private readonly Cache<string, string> _parentKeyProperties = new Cache<string, string>();
        private readonly Cache<string, string> _manyToManyProperties = new Cache<string, string>();
	    private readonly MethodInfo _collectionMethod;
        private readonly AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>> access;

	    public ManyToManyPart(PropertyInfo property)
            : this(property.Name, property.PropertyType)
	    {}

	    public ManyToManyPart(MethodInfo method)
	        : this(method.Name, method.ReturnType)
        {
	        _collectionMethod = method;
        }

        protected ManyToManyPart(string memberName, Type collectionType)
            : base(collectionType)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>>(this);
            _properties.Store("name", memberName);
        }

	    public ManyToManyPart<PARENT, CHILD> WithChildKeyColumn(string childKeyColumn)
		{
			_childKeyColumn = childKeyColumn;
			return this;
		}
		
		public ManyToManyPart<PARENT, CHILD> WithParentKeyColumn(string parentKeyColumn)
		{
			_parentKeyProperties.Store("column", parentKeyColumn);
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
            var conventions = visitor.Conventions;

            if (_collectionMethod != null)
            {
                var conventionName = conventions.GetReadOnlyCollectionBackingFieldName(_collectionMethod);
                _properties.Store("name", conventionName);
            }


			string tableName = GetTableName(conventions);
			string parentKeyName = GetParentKeyName(conventions);
			string childForeignKeyName = GetChildKeyName(conventions);

			XmlElement set = classElement.AddElement(_collectionType).WithProperties(_properties);
			set.WithAtt("table", tableName);

			XmlElement key = set.AddElement("key");
			key.WithAtt("column", parentKeyName);
		    key.WithProperties(_parentKeyProperties);

			XmlElement manyToManyElement = set.AddElement("many-to-many");
			manyToManyElement.WithAtt("column", childForeignKeyName);
			manyToManyElement.WithAtt("class", typeof(CHILD).AssemblyQualifiedName);
			manyToManyElement.WithProperties(_manyToManyProperties);
        }

		private string GetParentKeyName(Conventions conventions)
		{
			string parentKeyName;
			if (!_parentKeyProperties.Has("column"))
				parentKeyName = conventions.GetForeignKeyNameOfParent(typeof(PARENT));
			else
				parentKeyName = _parentKeyProperties.Get("column");
			return parentKeyName;
		}

		private string GetChildKeyName(Conventions conventions)
    	{
    		string childKeyName;
    		if (string.IsNullOrEmpty(_childKeyColumn))
				childKeyName = conventions.GetForeignKeyNameOfParent(typeof(CHILD));
			else
    			childKeyName = _childKeyColumn;
    		return childKeyName;
    	}

    	private string GetTableName(Conventions conventions)
    	{
    		string tableName;
    		if (string.IsNullOrEmpty(_tableName))
    			tableName = conventions.GetManyToManyTableName(typeof(CHILD), typeof(PARENT));
    		else
    			tableName = _tableName;
    		return tableName;
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
    }
}
