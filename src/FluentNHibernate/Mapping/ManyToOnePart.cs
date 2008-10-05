using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ManyToOnePart : IMappingPart, IAccessStrategy<ManyToOnePart>
    {
		private readonly Cache<string, string> _properties = new Cache<string, string>();
        private readonly PropertyInfo _property;
        private string _columnName;
        private readonly AccessStrategyBuilder<ManyToOnePart> access;

        public ManyToOnePart(PropertyInfo property) 
        {
            access = new AccessStrategyBuilder<ManyToOnePart>(this);

            _property = property;
        }

		public FetchTypeExpression<ManyToOnePart> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToOnePart>(this, _properties);
			}
		}
		
		public ManyToOnePart WithForeignKey()
		{
			return WithForeignKey(string.Format("FK_{0}To{1}", _property.DeclaringType.Name, _property.Name));
		}
		
		public ManyToOnePart WithForeignKey(string foreignKeyName)
		{
			_properties.Store("foreign-key", foreignKeyName);
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
            visitor.Conventions.AlterManyToOneMap(this);

            string columnName = _columnName;
            
            if (string.IsNullOrEmpty(_columnName))
                columnName = visitor.Conventions.GetForeignKeyName(_property);

        	_properties.Store("name", _property.Name);
			_properties.Store("column", columnName);

            classElement.AddElement("many-to-one").WithProperties(_properties);
        }

        public void SetAttribute(string name, string value)
        {
			_properties.Store(name, value);
        }

        public ManyToOnePart TheColumnNameIs(string name)
        {
            _columnName = name;

            return this;
        }

        public int Level
        {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        public AccessStrategyBuilder<ManyToOnePart> Access
        {
            get { return access; }
        }
    }
}
