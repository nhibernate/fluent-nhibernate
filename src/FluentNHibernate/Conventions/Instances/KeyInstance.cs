using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class KeyInstance : KeyInspector, IKeyInstance
    {
        private readonly KeyMapping mapping;

        public KeyInstance(KeyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void Column(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public void ForeignKey(string constraint)
        {
            if (!mapping.IsSpecified(x => x.ForeignKey))
                mapping.ForeignKey = constraint;
        }

        public new IEnumerable<IColumnInstance> Columns
        {
            get
            {
                return mapping.Columns.UserDefined
                    .Select(x => new ColumnInstance(mapping.ContainingEntityType, x))
                    .Cast<IColumnInstance>();
            }
        }
    }
}