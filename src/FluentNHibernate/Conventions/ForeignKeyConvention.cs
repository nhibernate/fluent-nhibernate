using System;
using System.Reflection;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public abstract class ForeignKeyConvention
        : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention, IJoinedSubclassConvention, IJoinConvention
    {
        protected abstract string GetKeyName(PropertyInfo property, Type type);

        public void Apply(IManyToOneInstance instance)
        {
            var columnName = GetKeyName(instance.Property, instance.Class.GetUnderlyingSystemType());

            instance.Column(columnName);
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            var keyColumn = GetKeyName(null, instance.EntityType);
            var childColumn = GetKeyName(null, instance.ChildType);

            if (instance.Key.Columns.IsEmpty())
                instance.Key.Column(keyColumn);

            if (instance.Relationship.Columns.IsEmpty())
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
    }
}