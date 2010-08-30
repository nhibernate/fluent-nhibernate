using System;
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
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();
            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void Type<T>()
        {
            Type(typeof(T));
        }

        public new void Type(Type type)
        {
            if (mapping.IsSpecified("Type"))
                return;

            mapping.Type = new TypeReference(type);
        }

        public new void Type(string type)
        {
            if (mapping.IsSpecified("Type"))
                return;

            mapping.Type = new TypeReference(type);
        }

        public new void Length(int length)
        {
            if (mapping.IsSpecified("Length"))
                return;

            mapping.Length = length;
        }
    }
}
