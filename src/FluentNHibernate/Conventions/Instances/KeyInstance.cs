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
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, Layer.Conventions, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new void ForeignKey(string constraint)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, constraint);
        }

        public new void PropertyRef(string property)
        {
            mapping.Set(x => x.PropertyRef, Layer.Conventions, property);
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

        public void CascadeOnDelete()
        {
            mapping.Set(x => x.OnDelete, Layer.Conventions, "cascade");
        }
    }
}