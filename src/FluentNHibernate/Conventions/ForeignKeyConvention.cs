using System;
using System.Reflection;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions
{
    public abstract class ForeignKeyConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        protected abstract string GetKeyName(PropertyInfo property, Type type);

        public void Accept(IAcceptanceCriteria<IManyToOneInspector> acceptance)
        {
            acceptance.Expect(x => x.Columns.IsEmpty());
        }

        public void Apply(IManyToOneInstance instance)
        {
            var columnName = GetKeyName(instance.Property, instance.Class ?? TypeReference.Empty);

            instance.ColumnName(columnName);
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.Key.Columns.IsEmpty());
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.ColumnName(columnName);
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.Key.Columns.IsEmpty() || x.ManyToMany.Columns.IsEmpty());
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            var keyColumn = GetKeyName(null, instance.EntityType);
            var childColumn = GetKeyName(null, instance.ChildType);

            if (instance.Key.Columns.IsEmpty())
                instance.Key.ColumnName(keyColumn);

            if (instance.ManyToMany.Columns.IsEmpty())
                instance.ManyToMany.ColumnName(childColumn);
        }
    }
}