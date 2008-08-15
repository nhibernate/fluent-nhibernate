using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
	public class ManyToManyPart<PARENT, CHILD> : IMappingPart
    {
    
        private readonly PropertyInfo _property;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
    	private string _tableName;
    	private string _childForeignKeyName;
    	private string _collectionType = "bag";
		private readonly Cache<string, string> _manyToManyProperties = new Cache<string, string>();

		public ManyToManyPart(PropertyInfo property)
        {
            _property = property;
            _properties.Add("name", _property.Name);
        }

        public CollectionCascadeExpression<ManyToManyPart<PARENT, CHILD>> Cascade
        {
			get { return new CollectionCascadeExpression<ManyToManyPart<PARENT, CHILD>>(this); }
        }

		public ManyToManyPart<PARENT, CHILD> WithTableName(string name)
		{
			_tableName = name;
			return this;
		} 
		
		public ManyToManyPart<PARENT, CHILD> WithChildForeignKey(string foreignKeyName)
		{
			_childForeignKeyName = foreignKeyName;
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

			string tableName = GetTableName(conventions);
			string parentForeignKeyName = conventions.GetForeignKeyNameOfParent(typeof(PARENT));
			string childForeignKeyName = GetChildForeignKeyName(conventions);

			XmlElement set = classElement.AddElement(_collectionType).WithProperties(_properties);
			set.WithAtt("table", tableName);
			set.WithAtt("name", _property.Name);

			XmlElement key = set.AddElement("key");
			key.WithAtt("column", parentForeignKeyName);

			XmlElement manyToManyElement = set.AddElement("many-to-many");
			manyToManyElement.WithAtt("column", childForeignKeyName);
			manyToManyElement.WithAtt("class", typeof(CHILD).AssemblyQualifiedName);
			manyToManyElement.WithProperties(_manyToManyProperties);
        }

    	private string GetChildForeignKeyName(Conventions conventions)
    	{
    		string childForeignKeyName;
    		if (string.IsNullOrEmpty(_childForeignKeyName))
				childForeignKeyName = conventions.GetForeignKeyNameOfParent(typeof(CHILD));
			else
    			childForeignKeyName = _childForeignKeyName;
    		return childForeignKeyName;
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
            _properties.Add(name, value);
        }

        public int Level
        {
            get { return 3; }
        }
    }
}
