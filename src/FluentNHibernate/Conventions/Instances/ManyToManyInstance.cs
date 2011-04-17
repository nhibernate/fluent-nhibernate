using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToManyInstance : ManyToManyInspector, IManyToManyInstance
    {
        private readonly ManyToManyMapping mapping;

        public ManyToManyInstance(ManyToManyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void Column(string columnName)
        {
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, Layer.Conventions, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new IEnumerable<IColumnInstance> Columns
        {
            get
            {
                return mapping.Columns
                    .Select(x => new ColumnInstance(mapping.ContainingEntityType, x))
                    .Cast<IColumnInstance>();
            }
        }

        public void CustomClass<T>()
        {
            CustomClass(typeof(T));
        }

        public void CustomClass(Type type)
        {
            mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
        }

        public new void ForeignKey(string constraint)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, constraint);
        }

        public new void Where(string where)
        {
            mapping.Set(x => x.Where, Layer.Conventions, where);
        }

        public new void OrderBy(string orderBy)
        {
            mapping.Set(x => x.OrderBy, Layer.Conventions, orderBy);
        }
    }
}
