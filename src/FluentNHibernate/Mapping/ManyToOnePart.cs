using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IManyToOnePart : IMappingPart
    {
        CascadeExpression<IManyToOnePart> Cascade { get; }
    }

    public class ManyToOnePart<OTHER> : IManyToOnePart, IAccessStrategy<ManyToOnePart<OTHER>>
    {
		private readonly Cache<string, string> _properties = new Cache<string, string>();
        private readonly PropertyInfo _property;
        private string _columnName;
        private readonly AccessStrategyBuilder<ManyToOnePart<OTHER>> access;

        public ManyToOnePart(PropertyInfo property) 
        {
            access = new AccessStrategyBuilder<ManyToOnePart<OTHER>>(this);

            _property = property;
        }

		public FetchTypeExpression<ManyToOnePart<OTHER>> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToOnePart<OTHER>>(this, _properties);
			}
		}

        public ManyToOnePart<OTHER> PropertyRef(Expression<Func<OTHER, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            _properties.Store("property-ref", prop.Name);

            return this;
        }

        public ManyToOnePart<OTHER> WithUniqueConstraint()
        {
            _properties.Store("unique", "true");
            return this;
        }

        public ManyToOnePart<OTHER> LazyLoad()
        {
            _properties.Store("lazy", "proxy");
            return this;
        }
		
		public ManyToOnePart<OTHER> WithForeignKey()
		{
			return WithForeignKey(string.Format("FK_{0}To{1}", _property.DeclaringType.Name, _property.Name));
		}
		
		public ManyToOnePart<OTHER> WithForeignKey(string foreignKeyName)
		{
			_properties.Store("foreign-key", foreignKeyName);
			return this;
		}
		
		public CascadeExpression<ManyToOnePart<OTHER>> Cascade
		{
			get
			{
				return new CascadeExpression<ManyToOnePart<OTHER>>(this);
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

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public ManyToOnePart<OTHER> TheColumnNameIs(string name)
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

        public ManyToOnePart<OTHER> CanNotBeNull()
        {
            this.SetAttribute("not-null", "true");
            return this;
        }

        public AccessStrategyBuilder<ManyToOnePart<OTHER>> Access
        {
            get { return access; }
        }

        public void PropertyRef(Func<object, object> func)
        {
            throw new NotImplementedException();
        }

        #region Explicit IManyToOnePart Implementation
        CascadeExpression<IManyToOnePart> IManyToOnePart.Cascade
        {
            get { return new CascadeExpression<IManyToOnePart>(this); }
        }
        #endregion
    }
}
