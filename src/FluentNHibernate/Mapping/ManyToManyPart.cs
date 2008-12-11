using System;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
	public class ManyToManyPart<PARENT, CHILD> : IMappingPart
    {
    
        private readonly Cache<string, string> _properties = new Cache<string, string>();
    	private string _tableName;
    	private string _childKeyColumn;
		private string _parentKeyColumn;
		private string _collectionType = "bag";
        private readonly Cache<string, string> _manyToManyProperties = new Cache<string, string>();
	    private readonly MethodInfo _collectionMethod;
        private readonly AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>> access;

	    public ManyToManyPart(PropertyInfo property)
            : this(property.Name)
	    {
	        SetDefaultCollectionType(property.PropertyType);
	    }

	    public ManyToManyPart(MethodInfo method)
	        : this(method.Name)
        {
	        _collectionMethod = method;
            SetDefaultCollectionType(method.ReturnType);
        }

	    private void SetDefaultCollectionType(Type type)
	    {
	        if (type.Namespace == "Iesi.Collections.Generic")
	            AsSet();
	    }

	    protected ManyToManyPart(string memberName)
        {
            access = new AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>>(this);
            _properties.Store("name", memberName);
        }

        public CollectionCascadeExpression<ManyToManyPart<PARENT, CHILD>> Cascade
        {
			get { return new CollectionCascadeExpression<ManyToManyPart<PARENT, CHILD>>(this); }
        }

        public ManyToManyPart<PARENT, CHILD> LazyLoad()
        {
            _properties.Store("lazy", "true");
            return this;
        }

        public ManyToManyPart<PARENT, CHILD> IsInverse()
        {
            _properties.Store("inverse", "true");
            return this;
        }

		public ManyToManyPart<PARENT, CHILD> WithTableName(string name)
		{
			_tableName = name;
			return this;
		} 
		
		public ManyToManyPart<PARENT, CHILD> WithChildKeyColumn(string childKeyColumn)
		{
			_childKeyColumn = childKeyColumn;
			return this;
		}
		
		public ManyToManyPart<PARENT, CHILD> WithParentKeyColumn(string parentKeyColumn)
		{
			_parentKeyColumn = parentKeyColumn;
			return this;
		}
		
		public ManyToManyPart<PARENT, CHILD> AsSet()
		{
			_collectionType = "set";
			return this;
		}

		public ManyToManyPart<PARENT, CHILD> AsBag()
		{
			_collectionType = "bag";
			return this;
		}

        /// <summary>
        /// Set the access and naming strategy for this one-to-many.
        /// </summary>
        public AccessStrategyBuilder<ManyToManyPart<PARENT, CHILD>> Access
        {
            get { return access; }
        }
		
		public FetchTypeExpression<ManyToManyPart<PARENT, CHILD>> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToManyPart<PARENT, CHILD>>(this, _manyToManyProperties);
			}
		}

		public void Write(XmlElement classElement, IMappingVisitor visitor)
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

			XmlElement manyToManyElement = set.AddElement("many-to-many");
			manyToManyElement.WithAtt("column", childForeignKeyName);
			manyToManyElement.WithAtt("class", typeof(CHILD).AssemblyQualifiedName);
			manyToManyElement.WithProperties(_manyToManyProperties);
        }

		private string GetParentKeyName(Conventions conventions)
		{
			string parentKeyName;
			if (string.IsNullOrEmpty(_parentKeyColumn))
				parentKeyName = conventions.GetForeignKeyNameOfParent(typeof(PARENT));
			else
				parentKeyName = _parentKeyColumn;
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
        public void SetAttribute(string name, string value)
        {
            _properties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int Level
        {
            get { return 3; }
        }

	    public PartPosition Position
	    {
            get { return PartPosition.Anywhere; }
	    }
    }
}
