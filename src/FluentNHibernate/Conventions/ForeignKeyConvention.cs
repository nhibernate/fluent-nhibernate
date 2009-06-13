using System;
using System.Reflection;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public abstract class ForeignKeyConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        protected abstract string GetKeyName(PropertyInfo property, Type type);

        public void Accept(IAcceptanceCriteria<IManyToOneInspector> acceptance)
        {
            acceptance.Expect(x => x.Columns.IsEmpty());
        }

        public void Apply(IManyToOneAlteration alteration, IManyToOneInspector inspector)
        {
            var columnName = GetKeyName(inspector.Property, inspector.Class);

            alteration.ColumnName(columnName);
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.Key.Columns.IsEmpty());
        }

        public void Apply(IOneToManyCollectionAlteration alteration, IOneToManyCollectionInspector inspector)
        {
            var columnName = GetKeyName(null, inspector.EntityType);
            
            alteration.Key.ColumnName(columnName);
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> acceptance)
        {
            acceptance.Expect(x => x.Key.Columns.IsEmpty() || x.ManyToMany.Columns.IsEmpty());
        }

        public void Apply(IManyToManyCollectionAlteration alteration, IManyToManyCollectionInspector inspector)
        {
            var keyColumn = GetKeyName(null, inspector.EntityType);
            var childColumn = GetKeyName(null, inspector.ChildType);

            if (inspector.Key.Columns.IsEmpty())
                alteration.Key.ColumnName(keyColumn);

            if (inspector.ManyToMany.Columns.IsEmpty())
                alteration.ManyToMany.ColumnName(childColumn);
        }
    }
}