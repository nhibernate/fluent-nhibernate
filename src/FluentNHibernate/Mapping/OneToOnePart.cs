using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class OneToOnePart : IMappingPart, IAccessStrategy<OneToOnePart>
    {
        private readonly Cache<string, string> _properties = new Cache<string, string>();
        private readonly PropertyInfo _property;
        private readonly AccessStrategyBuilder<OneToOnePart> access;

        public OneToOnePart(PropertyInfo property) {
            access = new AccessStrategyBuilder<OneToOnePart>(this);
            _property = property;
        }

        public FetchTypeExpression<OneToOnePart> FetchType {
            get {
                return new FetchTypeExpression<OneToOnePart>(this, _properties);
            }
        }

        public OneToOnePart WithForeignKey() {
            return WithForeignKey(string.Format("FK_{0}To{1}", _property.DeclaringType.Name, _property.Name));
        }

        public OneToOnePart WithForeignKey(string foreignKeyName) {
            _properties.Store("foreign-key", foreignKeyName);
            return this;
        }

        public CascadeExpression<OneToOnePart> Cascade {
            get {
                return new CascadeExpression<OneToOnePart>(this);
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

        public AccessStrategyBuilder<OneToOnePart> Access {
            get { return access; }
        }
    }
}
