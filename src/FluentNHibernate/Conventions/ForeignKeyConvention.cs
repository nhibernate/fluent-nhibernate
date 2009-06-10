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

        public void Accept(IAcceptanceCriteria<IOneToManyInspector> acceptance)
        {
            acceptance.Expect(x => x.KeyColumns.IsEmpty());
        }

        public void Apply(IOneToManyAlteration alteration, IOneToManyInspector inspector)
        {
            var columnName = GetKeyName(null, inspector.EntityType);

            alteration.KeyColumnName(columnName);
        }

        public void Accept(IAcceptanceCriteria<IManyToManyInspector> acceptance)
        {
            acceptance.Expect(x => x.ParentKeyColumns.IsEmpty() || x.ChildKeyColumns.IsEmpty());
        }

        public void Apply(IManyToManyAlteration alteration, IManyToManyInspector inspector)
        {
            var columnName = GetKeyName(null, inspector.EntityType);

            if (inspector.ParentKeyColumns.IsEmpty())
                alteration.ParentKeyColumn(columnName);

            if (inspector.ChildKeyColumns.IsEmpty())
                alteration.ChildKeyColumn(columnName);
        }
    }
}