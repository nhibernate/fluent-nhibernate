using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToManyInstance : RelationshipInstance, IManyToManyInstance
    {
        private new readonly ManyToManyMapping mapping;

        public ManyToManyInstance(ManyToManyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void ColumnName(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public IDefaultableEnumerable<IColumnInstance> Columns
        {
            get
            {
                var items = new DefaultableList<IColumnInstance>();

                foreach (var column in mapping.Columns)
                {
                    items.Add(new ColumnInstance(mapping.ParentType, column));
                }

                return items;
            }
        }

        IDefaultableEnumerable<IColumnInspector> IManyToManyInspector.Columns
        {
            get
            {
                var items = new DefaultableList<IColumnInspector>();

                foreach (var column in mapping.Columns)
                {
                    items.Add(new ColumnInspector(mapping.ParentType, column));
                }

                return items;
            }
        }
    }
}