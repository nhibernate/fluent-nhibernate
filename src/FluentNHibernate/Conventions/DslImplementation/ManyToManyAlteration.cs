using System;
using System.Linq;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ManyToManyAlteration : IManyToManyAlteration
    {
        private readonly ManyToManyMapping mapping;

        public ManyToManyAlteration(ManyToManyMapping mapping)
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