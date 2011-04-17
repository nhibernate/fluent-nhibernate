using System;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public abstract class ForeignKeyConvention
        : IReferenceConvention, IHasManyToManyConvention, IJoinedSubclassConvention, IJoinConvention, ICollectionConvention
    {
        protected abstract string GetKeyName(Member property, Type type);

        public void Apply(IManyToOneInstance instance)
        {
            var columnName = GetKeyName(instance.Property, instance.Class.GetUnderlyingSystemType());

            instance.Column(columnName);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            var keyColumn = GetKeyName(null, instance.EntityType);
            var childColumn = GetKeyName(null, instance.ChildType);

            instance.Key.Column(keyColumn);
            instance.Relationship.Column(childColumn);
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            var columnName = GetKeyName(null, instance.Type.BaseType);
            
            instance.Key.Column(columnName);
        }

        public void Apply(IJoinInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }

        public void Apply(ICollectionInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }
    }
}