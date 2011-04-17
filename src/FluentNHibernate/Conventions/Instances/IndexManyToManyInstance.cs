using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class IndexManyToManyInstance :IndexManyToManyInspector, IIndexManyToManyInstance
    {
        private readonly IndexManyToManyMapping mapping;

        public IndexManyToManyInstance(IndexManyToManyMapping mapping) 
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
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();
            column.Set(x => x.Name, Layer.Conventions, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new void ForeignKey(string foreignKey)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, foreignKey);
        }
    }
}
