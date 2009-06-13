using System.Linq;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class KeyAlteration : IKeyAlteration
    {
        private readonly KeyMapping mapping;

        public KeyAlteration(KeyMapping mapping)
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