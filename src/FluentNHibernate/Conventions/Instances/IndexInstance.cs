using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class IndexInstance : IndexInspector, IIndexInstance
    {
        private readonly IndexMapping mapping;

        public IndexInstance(IndexMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Adds a column to the index if columns have not yet been specified
        /// </summary>
        /// <param name="columnName">The column name to add</param>
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
    }
}
