using System;
using System.Linq;

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ElementInstance : ElementInspector, IElementInstance
    {
        private readonly ElementMapping mapping;

        public ElementInstance(ElementMapping mapping)
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

        public new void Type<T>()
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(typeof(T));
        }

        public new void Type(string type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }

        public new void Type(Type type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }
    }
}