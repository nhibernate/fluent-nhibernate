using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IOneToOnePart : IMappingPart
    {
        CascadeExpression<IOneToOnePart> Cascade { get; }
    }

    public class OneToOnePart<OTHER> : IOneToOnePart, IAccessStrategy<OneToOnePart<OTHER>>
    {
        private readonly Cache<string, string> _properties = new Cache<string, string>();
        private readonly PropertyInfo _property;
        private readonly AccessStrategyBuilder<OneToOnePart<OTHER>> access;

        public OneToOnePart(PropertyInfo property) {
            access = new AccessStrategyBuilder<OneToOnePart<OTHER>>(this);
            _property = property;
        }

        public FetchTypeExpression<OneToOnePart<OTHER>> FetchType {
            get {
                return new FetchTypeExpression<OneToOnePart<OTHER>>(this, _properties);
            }
        }
        
        public OneToOnePart<OTHER> WithForeignKey() {
            return WithForeignKey(string.Format("FK_{0}To{1}", _property.DeclaringType.Name, _property.Name));
        }

        public OneToOnePart<OTHER> WithForeignKey(string foreignKeyName) {
            _properties.Store("foreign-key", foreignKeyName);
            return this;
        }

        public OneToOnePart<OTHER> PropertyRef(Expression<Func<OTHER, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            _properties.Store("property-ref", prop.Name);

            return this;
        }

        public CascadeExpression<OneToOnePart<OTHER>> Cascade
        {
            get {
                return new CascadeExpression<OneToOnePart<OTHER>>(this);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor) {
            visitor.RegisterDependency(_property.PropertyType);
            visitor.Conventions.AlterOneToOneMap(this);

            _properties.Store("name", _property.Name);
            _properties.Store("class", _property.PropertyType.AssemblyQualifiedName);

            classElement.AddElement("one-to-one").WithProperties(_properties);
        }

        public void SetAttribute(string name, string value) {
            _properties.Store(name, value);
        }

        public int Level {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        public AccessStrategyBuilder<OneToOnePart<OTHER>> Access {
            get { return access; }
        }

        #region Explicit IOneToOnePart Implementation
        CascadeExpression<IOneToOnePart> IOneToOnePart.Cascade
        {
            get { return new CascadeExpression<IOneToOnePart>(this); }
        }
        #endregion
    }
}
