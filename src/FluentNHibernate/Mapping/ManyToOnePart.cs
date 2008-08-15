using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ManyToOnePart : IMappingPart, IAccessStrategy<ManyToOnePart>
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly PropertyInfo _property;
        private readonly string _columnName;
        private readonly AccessStrategyBuilder<ManyToOnePart> access;
    	private FetchType _fetchType = FetchType.Join;

    	public ManyToOnePart(PropertyInfo property) : this(property, null){}

        public ManyToOnePart(PropertyInfo property, string columnName) 
        {
            access = new AccessStrategyBuilder<ManyToOnePart>(this);

            _property = property;

            _columnName = columnName;
        }

		public ManyToOnePart WithFetchType(FetchType fetchType)
		{
			_fetchType = fetchType;
			return this;
		}
		
		public ManyToOnePart WithForeignKey()
		{
			return WithForeignKey(string.Format("FK_{0}To{1}", _property.DeclaringType.Name, _property.Name));
		}
		
		public ManyToOnePart WithForeignKey(string foreignKeyName)
		{
			_properties.Add("foreign-key", foreignKeyName);
			return this;
		}
		
		public CascadeExpression<ManyToOnePart> Cascade
		{
			get
			{
				return new CascadeExpression<ManyToOnePart>(this);
			}
		}

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            visitor.RegisterDependency(_property.PropertyType);

            string columnName = _columnName;
            
            if (string.IsNullOrEmpty(_columnName))
                columnName = visitor.Conventions.GetForeignKeyName(_property);

			_properties["name"] = _property.Name;
			_properties["column"] = columnName;
			_properties["fetch"] = _fetchType.Type;

            classElement.AddElement("many-to-one").WithProperties(_properties);
        }

        public void SetAttribute(string name, string value)
        {
            _properties.Add(name, value);
        }

        public int Level
        {
            get { return 3; }
        }

        public AccessStrategyBuilder<ManyToOnePart> Access
        {
            get { return access; }
        }
    }
}
