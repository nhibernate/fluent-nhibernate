using System.Reflection;

namespace FluentNHibernate.Testing.Values
{
    public class ReferenceProperty<T, TProperty> : Property<T, TProperty>
    {
        public ReferenceProperty(PropertyInfo property, TProperty propertyValue) : base(property, propertyValue)
        {}

        public override void HasRegistered(PersistenceSpecification<T> specification)
        {
            specification.TransactionalSave(Value);
        }
    }
}