using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
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
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new IDefaultableEnumerable<IColumnInstance> Columns
        {
            get
            {
                return mapping.Columns.UserDefined
                    .Select(x => new ColumnInstance(mapping.ContainingEntityType, x))
                    .Cast<IColumnInstance>()
                    .ToDefaultableList();
            }
        }

        public void CustomClass<T>()
        {
            mapping.Class = new TypeReference(typeof(T));
        }

        public void CustomClass(Type type)
        {
            mapping.Class = new TypeReference(type);
        }

        public new void ForeignKey(string constraint)
        {
            if (!mapping.IsSpecified(x => x.ForeignKey))
                mapping.ForeignKey = constraint;
        }

        public new void Where(string where)
        {
            if (!mapping.IsSpecified(x => x.Where))
                mapping.Where = where;
        }

        public new void OrderBy(string orderBy)
        {
            if (!mapping.IsSpecified(x => x.OrderBy))
                mapping.OrderBy = orderBy;
        }
    }
}
