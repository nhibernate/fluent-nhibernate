using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

public class ReferenceProperty<T, TProperty>(Accessor property, TProperty propertyValue)
    : Property<T, TProperty>(property, propertyValue)
{
    public override void HasRegistered(PersistenceSpecification<T> specification)
    {
        specification.TransactionalSave(Value);
    }
}
