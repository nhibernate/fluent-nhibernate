using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
{
    public class ManyToOneAlteration : IManyToOneAlteration
    {
        private readonly ManyToOneMapping mapping;

        public ManyToOneAlteration(ManyToOneMapping mapping)
        {
            this.mapping = mapping;
        }

        public void ColumnName(string columnName)
        {
            var column = mapping.Columns.FirstOrDefault();
            var columnAttributes = column == null ? new AttributeStore<ColumnMapping>() : column.Attributes.Clone();

            mapping.ClearColumns();
            mapping.AddColumn(new ColumnMapping(columnAttributes) { Name = columnName });
        }
    }
}